using BE.Common;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Common
{
    public class SystemParameterBL
    {
        private DatabaseContext ctx = new DatabaseContext();

        public List<Dropdownlist> GetParametroByGrupoId(int grupoId)
        {
            var isDeleted = (int)Enumeratores.SiNo.No;
            List<Dropdownlist> result = (from a in ctx.SystemParameter
                                         where a.i_IsDeleted == isDeleted && a.i_GroupId == grupoId
                                         orderby a.i_Sort ascending
                                         select new Dropdownlist
                                         {
                                             Id = a.i_ParameterId,
                                             Value = a.v_Value1,
                                             Field = a.v_Field
                                         }).OrderBy(x => x.Id).ToList();
            return result;
        }

        public List<Dropdownlist> GetParametroMasterServiceByGrupoId(int serviceType)
        {

            List<Dropdownlist> result = (from a in ctx.NodeServiceProfile
                                         join c in ctx.SystemParameter on new { a = a.i_MasterServiceId.Value, b = 119 }
                                         equals new { a = c.i_ParameterId, b = c.i_GroupId }
                                         where a.i_IsDeleted == 0 && a.i_NodeId == 9 && a.i_ServiceTypeId == serviceType
                                         select new Dropdownlist
                                         {
                                             Id = c.i_ParameterId,
                                             Value = c.v_Value1
                                         }).OrderBy(x => x.Value).Distinct().ToList();
            return result;
        }

        public List<Dropdownlist> GetParameterTypeServiceByGrupoId(int grupoId)
        {

            List<Dropdownlist> result = (from a in ctx.NodeServiceProfile
                                         join c in ctx.SystemParameter on new { a = a.i_ServiceTypeId.Value, b = grupoId }
                                         equals new { a = c.i_ParameterId, b = c.i_GroupId }
                                         where a.i_NodeId == 9
                                         
                                         select new Dropdownlist
                                         {
                                             Id = c.i_ParameterId,
                                             Value = c.v_Value1
                                         }).OrderBy(x => x.Value).Distinct().ToList();
            return result;
        }

        public List<Dropdownlist> GetPuestos()
        {
            List<Dropdownlist> result = (from a in ctx.Person
                                         where a.i_IsDeleted == 0
                                         select new Dropdownlist
                                         {
                                             Value = a.v_CurrentOccupation
                                         }).OrderBy(x => x.Value).Distinct().ToList();
            return result;
        }

        public List<Dropdownlist> EmpresaFacturacion(int nodo)
        {
            List<Dropdownlist> result = (from nod in ctx.Node
                                         join norg in ctx.NodeOrganizationLocationProfile on nod.i_NodeId equals norg.i_NodeId
                                         join npfl in ctx.NodeOrganizationProfile on norg.v_OrganizationId equals npfl.v_OrganizationId
                                         join org in ctx.Organization on npfl.v_OrganizationId equals org.v_OrganizationId
                                         where nod.i_IsDeleted == 0 && nod.i_NodeId == nodo && norg.i_IsDeleted == 0
                                         select new Dropdownlist
                                         {
                                             v_Id = org.v_OrganizationId,
                                             Value = org.v_Name
                                         }).OrderBy(x => x.Value).Distinct().ToList();
            return result;
        }
        
        public List<Dropdownlist> GetGeso(string organizationId, string locationId)
        {
            List<Dropdownlist> result = (from gro in ctx.GroupOccupation
                                         join loc in ctx.Location on gro.v_LocationId equals loc.v_LocationId
                                         join org in ctx.Organization on loc.v_OrganizationId equals org.v_OrganizationId
                                         where gro.i_IsDeleted == 0 && loc.v_LocationId == locationId && org.v_OrganizationId == organizationId
                                         select new Dropdownlist
                                         {
                                             v_Id = gro.v_GroupOccupationId,
                                             Value = gro.v_Name
                                         }).OrderBy(x => x.Value).ToList();
            return result;
        }

        public List<Dropdownlist> GetGeso(string organizationId)
        {
            List<Dropdownlist> result = (from gro in ctx.GroupOccupation
                                         join loc in ctx.Location on gro.v_LocationId equals loc.v_LocationId
                                         join org in ctx.Organization on loc.v_OrganizationId equals org.v_OrganizationId
                                         where gro.i_IsDeleted == 0  && org.v_OrganizationId == organizationId
                                         select new Dropdownlist
                                         {
                                             v_Id = gro.v_GroupOccupationId,
                                             Value = gro.v_Name
                                         }).OrderBy(x => x.Value).ToList();
            return result;
        }
        public List<Dropdownlist> GetProtocolsForCombo(int Service, int ServiceType)
        {
            List<Dropdownlist> result = (from pro in ctx.Protocol
                                         where pro.i_IsDeleted == 0 && pro.i_MasterServiceTypeId == ServiceType && pro.i_MasterServiceId == Service
                                         select new Dropdownlist
                                         {
                                             v_Id = pro.v_ProtocolId,
                                             Value = pro.v_Name
                                         }).OrderBy(x => x.Value).ToList();
            return result;
        }

        public List<Dropdownlist> GetProtocolsForComboEspeciality(int Service, int ServiceType)
        {

            List<Dropdownlist> result = (from pro in ctx.Protocol
                                         where pro.i_IsDeleted == 0 && pro.i_MasterServiceTypeId == ServiceType && pro.i_MasterServiceId == Service
                                         select new Dropdownlist
                                         {
                                             v_Id = pro.v_ProtocolId,
                                             Value = pro.v_Name                                             
                                         }).OrderBy(x => x.Value).ToList();

            foreach (var item in result)
            {
                var listProtComp = ctx.ProtocolComponent.Where(x => x.v_ProtocolId == item.v_Id).ToList();
                if (listProtComp.Count > 0)
                {
                    var precioTotal = listProtComp.Sum(x => x.r_Price);
                    item.Field = precioTotal.ToString("N2");
                }

            }   

            return result;
        }

        public List<Dropdownlist> GetOrganizationAndLocation(int nodeId)
        {
            try
            {
                List<Dropdownlist> result = (from nod in ctx.Node
                                             join norg in ctx.NodeOrganizationLocationProfile on nod.i_NodeId equals norg.i_NodeId
                                             join nopf in ctx.NodeOrganizationProfile on new { a = norg.i_NodeId, b = norg.v_OrganizationId } equals new { a = nopf.i_NodeId, b = nopf.v_OrganizationId }
                                             join org in ctx.Organization on nopf.v_OrganizationId equals org.v_OrganizationId
                                             join loc in ctx.Location on norg.v_LocationId equals loc.v_LocationId
                                             where nod.i_NodeId == nodeId && nod.i_IsDeleted == 0 && norg.i_IsDeleted == 0
                                             select new Dropdownlist
                                             {
                                                 v_Id = norg.v_OrganizationId + "|" + norg.v_LocationId,
                                                 Value = org.v_Name + " / Sede: " + loc.v_Name
                                             }).OrderBy(x => x.Value).ToList();
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
            
        }


        public List<Dropdownlist> GetUser()
        {
            List<Dropdownlist> result = (from sys in ctx.SystemUser
                                         join per in ctx.Person on sys.v_PersonId equals per.v_PersonId
                                         where sys.i_IsDeleted == 0 
                                         select new Dropdownlist
                                         {
                                             Id = sys.i_SystemUserId,
                                             Value = per.v_FirstLastName + " " + per.v_SecondLastName + ", " + per.v_FirstName,
                                         }).OrderBy(x => x.Value).ToList();
            return result;
        }

        public List<Dropdownlist> GetCentroCosto()
        {
            List<Dropdownlist> result = (from ser in ctx.Service
                                         where ser.i_IsDeleted == 0
                                         select new Dropdownlist
                                         {
                                             v_Id = ser.v_centrocosto,
                                             Value = ser.v_centrocosto,
                                         }).OrderBy(x => x.Value).Distinct().ToList();
            return result;
        }

        public List<Dropdownlist> GetTitulares()
        {
            List<Dropdownlist> result = (from per in ctx.Person
                                         join pac in ctx.Pacient on per.v_PersonId equals pac.v_PersonId
                                         where per.i_IsDeleted == 0
                                         select new Dropdownlist
                                         {
                                             v_Id = per.v_PersonId,
                                             Value = per.v_FirstLastName + " " + per.v_SecondLastName + " " + per.v_FirstName,
                                         }).OrderBy(x => x.Value).Distinct().ToList();
            return result;
        }

        public List<Dropdownlist> GetOrganizationForCombo()
        {
            //mon.IsActive = true;
            try
            {
                DatabaseContext dbContext = new DatabaseContext();
                List<Dropdownlist> DataComponentList;
                DataComponentList = (from a in dbContext.Organization
                                     where a.i_IsDeleted == 0 && a.i_OrganizationTypeId == 4
                                     select a).AsEnumerable()
                                           .Select(x => new Dropdownlist
                                           {
                                               Value = x.v_Name,
                                               v_Id = x.v_OrganizationId
                                           }).OrderBy(x => x.Value).ToList();


                return DataComponentList;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
