using BE.Security;
using DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Linq;
using System.Net.Sockets;
using DAL.Organizarion;
using static BE.Common.Enumeratores;

namespace BL.Security
{
   public class AuthorizationBL
    {
        private DatabaseContext ctx = new DatabaseContext();

        //Refactoring: la consulta debería estar en la capa de acceso a datos
        public AuthorizationModel ValidateSystemUser(int nodeId, string userName, string password)
        {
            var isWorker = int.TryParse(userName, out _);

            return isWorker ? ProcessWorkerUser(nodeId, userName, password) : ProcessSystemUser(nodeId, userName, password);
        }

        private AuthorizationModel ProcessWorkerUser(int nodeId, string userName, string password)
        {
            var isDeleted = (int)SiNo.No;

            var workerUser = (from wor in ctx.Pacient
                join per in ctx.Person on wor.v_PersonId equals per.v_PersonId
                where wor.i_IsDeleted == isDeleted 
                        && per.i_IsDeleted == isDeleted
                        && per.v_DocNumber == userName
                        && per.v_Password == password
                select new AuthorizationModel
                {
                    PersonId = per.v_PersonId,
                    FullName = per.v_FirstName + " " + per.v_FirstLastName + " " +  per.v_SecondLastName,
                    PersonImage = per.b_PersonImage,
                    UserName = per.v_FirstName + " " + per.v_FirstLastName,
                    //SystemUserId = sys.i_SystemUserId.Value,
                    //SystemUserTypeId = sys.i_SystemUserTypeId.Value,
                    //SystemUserByOrganizationId = sys.v_SystemUserByOrganizationId
                }).FirstOrDefault();

            return workerUser;
        }

        private AuthorizationModel ProcessSystemUser(int nodeId, string userName, string password)
        {
            var isDeleted = (int)SiNo.No;
            var user = (from sys in ctx.SystemUser
                join per in ctx.Person on sys.v_PersonId equals per.v_PersonId
                join pro in ctx.Professional on per.v_PersonId equals pro.v_PersonId into proJoin
                from pro in proJoin.DefaultIfEmpty()
                join org in ctx.Organization on sys.v_SystemUserByOrganizationId equals org.v_OrganizationId into orgJoin
                from org in orgJoin.DefaultIfEmpty()
                where sys.v_UserName == userName &&
                      sys.v_Password == password &&
                      sys.i_IsDeleted == isDeleted
                select new AuthorizationModel
                {
                    PersonId = sys.v_PersonId,
                    FullName = per.v_FirstName + " " + per.v_FirstLastName,
                    PersonImage = per.b_PersonImage,
                    UserName = sys.v_UserName,
                    RucEmpresa = org.v_IdentificationNumber,
                    SystemUserId = sys.i_SystemUserId.Value,
                    SystemUserTypeId = sys.i_SystemUserTypeId.Value,
                    SystemUserByOrganizationId = sys.v_SystemUserByOrganizationId
                }).FirstOrDefault();

            if (user == null) return null;

            user.Permissions = GetPermissions(nodeId, user.SystemUserId);
            user.Organizations = GetOrganization(user.SystemUserByOrganizationId);
            user.Options = GetOptions(nodeId, user.SystemUserId);
            return user;
        }

        private List<OrganizationSystemUser> GetOrganization(string systemUserByOrganizationId)
        {
            var array = systemUserByOrganizationId.Split(',');
            var listIds = new List<string>(array);
            var organizations = new OrganizationDal().GetOrganizationByIds(listIds);
            var list = new List<OrganizationSystemUser>();

            foreach (var item in array)
            {
                var oOrganizationSystemUser = new OrganizationSystemUser();
                
                var org = organizations.Find(p => p.v_OrganizationId == item.Trim());
                oOrganizationSystemUser.Name = org.v_Name;
                oOrganizationSystemUser.OrganizationId = org.v_OrganizationId;
                oOrganizationSystemUser.WareHouses = new OrganizationDal().GetWareHouses(org.v_OrganizationId);
                list.Add(oOrganizationSystemUser);
            }

            return list;

        }

        public List<Permission> GetPermissions(int nodeId, int systemUserId)
        {
            var isDeleted = (int)SiNo.No;
                var query = (from rnp in ctx.RoleNodeProfile
                             join rn in ctx.RoleNode on new { a = rnp.i_NodeId, b = rnp.i_RoleId }
                                                     equals new { a = rn.i_NodeId, b = rn.i_RoleId } into rnJoin
                             from rnj in rnJoin.DefaultIfEmpty()

                             join surn in ctx.SystemUserRoleNode on new { a = rnp.i_NodeId, b = rnp.i_RoleId }
                                                    equals new { a = surn.i_NodeId, b = surn.i_RoleId } into surnJoin
                             from surnj in surnJoin.DefaultIfEmpty()

                             join ah in ctx.ApplicationHierarchy on rnp.i_ApplicationHierarchyId equals ah.i_ApplicationHierarchyId

                             join fff in ctx.SystemParameter on new { a = surnj.i_RoleId.Value, b = 115 } // ROLES DEL SISTEMA
                                                                   equals new { a = fff.i_ParameterId, b = fff.i_GroupId } into j5Join
                             from fff in j5Join.DefaultIfEmpty()

                             where (surnj.i_NodeId == nodeId)
                                   && (surnj.i_SystemUserId == systemUserId)
                                   && (ah.i_ApplicationHierarchyTypeId == 2 || ah.i_ApplicationHierarchyTypeId == 1)
                                    && (surnj.i_IsDeleted == isDeleted) && (rnp.i_IsDeleted == isDeleted)
                                   && (ah.i_TypeFormId == (int)TypeForm.Web) && (ah.i_IsDeleted == isDeleted)
                             select new Permission
                             {
                                 ApplicationHierarchyId = rnp.i_ApplicationHierarchyId.Value,
                                 ApplicationHierarchyTypeId = ah.i_ApplicationHierarchyTypeId.Value,
                                 Description = ah.v_Description,
                                 ParentId = ah.i_ParentId.Value,
                                 Form = ah.v_Form == null ? string.Empty : ah.v_Form,
                                 RoleName = fff.v_Value1,
                                 RoleId = fff.i_ParameterId
                             }
                          )
                  .Concat(from a in ctx.SystemUserGobalProfile
                          join b in ctx.ApplicationHierarchy on a.i_ApplicationHierarchyId equals b.i_ApplicationHierarchyId
                          where (a.i_SystemUserId == systemUserId)
                               && (b.i_ApplicationHierarchyTypeId == 1 || b.i_ApplicationHierarchyTypeId == 2)
                                && (b.i_IsDeleted == 0) && (a.i_IsDeleted == 0)
                               && (b.i_TypeFormId == (int)TypeForm.Web)
                          select new Permission
                          {
                              ApplicationHierarchyId = a.i_ApplicationHierarchyId.Value,
                              ApplicationHierarchyTypeId = b.i_ApplicationHierarchyTypeId.Value,
                              Description = b.v_Description,
                              ParentId = b.i_ParentId.Value,
                              Form = b.v_Form == null ? string.Empty : b.v_Form,
                              RoleName = "",
                              RoleId = 0
                          }).ToList();

                List<Permission> objAutorizationList = query.AsEnumerable()
                                                              .OrderBy(p => p.ApplicationHierarchyId)
                                                              .GroupBy(x => x.ApplicationHierarchyId)
                                                              .Select(group => group.First())
                                                              .ToList();

                var parents = objAutorizationList.FindAll(p => p.ParentId == -1);

                var result = new List<Permission>();
               
                foreach (var parent in parents)
                {
                    var oPermission = new Permission();
                    oPermission.ApplicationHierarchyId = parent.ApplicationHierarchyId;
                    oPermission.ApplicationHierarchyTypeId = parent.ApplicationHierarchyTypeId;
                    oPermission.Description = parent.Description;
                    oPermission.ParentId = parent.ParentId;
                    oPermission.Form = parent.Form;
                    oPermission.RoleName = parent.RoleName;
                    oPermission.RoleId = parent.RoleId;
                    
                    LoadTreeSubMenuPermission(ref oPermission, query, parent.ApplicationHierarchyId); 

                    result.Add(oPermission);
                }

                return result;
        }

        public List<Option> GetOptions(int nodeId, int systemUserId)
        {
            using (var ctx = new DatabaseContext())
            {
                var query = (from A in ctx.SystemUserGobalProfile
                    join B in ctx.AplicationHierarchy on A.i_ApplicationHierarchyId equals B.i_ApplicationHierarchyId
                    where A.i_SystemUserId == 87 && B.i_ApplicationHierarchyTypeId == 4 && A.i_IsDeleted == 0
                    select new Option
                    {
                        Name = B.v_Description,
                        path = B.v_Form

                    }).ToList();

                return query;
            }

        }

        private void LoadTreeSubMenuPermission(ref Permission parentNode, List<Permission> permissions, int parentId)
        {
            var submenus = permissions.FindAll(p => p.ParentId == parentId);

            //var subMenus = new List<Permission>();
            foreach (var submenu in submenus)
            {
                var subMenu = new Permission();

                subMenu.Description = submenu.Description;
                subMenu.Form = submenu.Form;
                if (parentNode.SubMenus == null)
                {
                    parentNode.SubMenus = new List<Permission>();
                }
                parentNode.SubMenus.Add(subMenu);

                LoadTreeSubMenuPermission(ref subMenu, permissions, submenu.ApplicationHierarchyId);
            }
            //return subMenus;
        }

        [Obsolete("Ahora los permisos son a nivel de SystemUserGlobalProfile")]
        public List<Permission> GetPermissionsExternal(int nodeId, int systemUserId)
        {
            try
            {
                var permissions = GetPermissions(nodeId, systemUserId);

                permissions[0].SubMenus.RemoveAll(r => r.Description == "Módulo de Farmacia" || r.Description == "Módulo de Reportes");

                return permissions;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

    }
}
