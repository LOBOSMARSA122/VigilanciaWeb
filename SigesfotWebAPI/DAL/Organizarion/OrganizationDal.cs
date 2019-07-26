using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using BE.Common;
using BE.Organization;
using BE.Security;
using BE.Warehouse;
using static BE.Common.Enumeratores;

namespace DAL.Organizarion
{
   public  class OrganizationDal
    {
        private static DatabaseContext ctx = new DatabaseContext();

        public static string SaveOrganization(OrganizationBE data, int userId, int nodeId)
        {
            try
            {
                using (var ts = new TransactionScope())
                {
                    string organization = GetOrganizationByRuc(data.v_IdentificationNumber);

                    if (organization != null) return organization;

                    string newId = "";
                    newId = new Common.Utils().GetPrimaryKey(nodeId, 5, "OO");
                    data.v_OrganizationId = newId;
                    data.i_InsertUserId = userId;
                    data.d_InsertDate = DateTime.Now;
                    data.i_IsDeleted = 0;
                    ctx.Organization.Add(data);
                    ctx.SaveChanges();

                    LocationBE loc = new LocationBE();
                    var arr = data.v_Address.Split('-').Reverse().ToArray();
                    var sede = arr[0].ToString();

                    loc.v_OrganizationId = newId;
                    loc.v_Name = sede;
                    string locationId = SaveLocationAndGroupOccupation(loc, userId, nodeId);
                    if (locationId == null) throw new Exception("Error");

                    ts.Complete();
                    return newId + "|" + locationId;
                }
                
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private static string SaveLocationAndGroupOccupation(LocationBE data, int userId, int nodeId)
        {
            try
            {

                //Location
                string newId = "";
                newId = new Common.Utils().GetPrimaryKey(nodeId,14,"OL");
                data.v_LocationId = newId;
                data.i_InsertUserId = userId;
                data.d_InsertDate = DateTime.Now;
                data.i_IsDeleted = 0;
                ctx.Location.Add(data);
                ctx.SaveChanges();


                //NodeOrganizationProfile
                NodeOrganizationProfileBE _NodeOrgProf = new NodeOrganizationProfileBE();
                _NodeOrgProf.i_NodeId = nodeId;
                _NodeOrgProf.v_OrganizationId = data.v_OrganizationId;
                _NodeOrgProf.i_InsertUserId = userId;
                _NodeOrgProf.d_InsertDate = DateTime.Now;
                _NodeOrgProf.i_IsDeleted = 0;
                ctx.NodeOrganizationProfile.Add(_NodeOrgProf);
                ctx.SaveChanges();

                //NodeOrganizationLocationProfile
                NodeOrganizationLocationProfileBE _NodeOrgLocProf = new NodeOrganizationLocationProfileBE();

                _NodeOrgLocProf.v_LocationId = newId;
                _NodeOrgLocProf.v_OrganizationId = data.v_OrganizationId;
                _NodeOrgLocProf.i_NodeId = nodeId;
                _NodeOrgLocProf.i_InsertUserId = userId;
                _NodeOrgLocProf.d_InsertDate = DateTime.Now;
                _NodeOrgLocProf.i_IsDeleted = 0;
                ctx.NodeOrganizationLocationProfile.Add(_NodeOrgLocProf);
                ctx.SaveChanges();


                ////NodeOrganizationLocationWarehouseProfile
                //NodeOrganizationLocationWarehouseProfileBE _NodeOrgLocWarProf = new NodeOrganizationLocationWarehouseProfileBE();
                //_NodeOrgLocWarProf.v_WarehouseId = "";
                //_NodeOrgLocWarProf.i_NodeId = nodeId;
                //_NodeOrgLocWarProf.v_OrganizationId = data.v_OrganizationId;
                //_NodeOrgLocWarProf.v_LocationId = data.v_LocationId;
                //_NodeOrgLocWarProf.i_InsertUserId = userId;
                //_NodeOrgLocWarProf.d_InsertDate = DateTime.Now;
                //_NodeOrgLocWarProf.i_IsDeleted = 0;
                //ctx.NodeOrganizationLocationWarehouseProfile.Add(_NodeOrgLocWarProf);
                //ctx.SaveChanges();


                //GroupOccupation
                List<string> Groups = new List<string>();
                Groups.Add("ADMINISTRATIVO");
                Groups.Add("OPERARIO");

                foreach (var geso in Groups)
                {
                    string newIdGroup = new Common.Utils().GetPrimaryKey(nodeId, 13, "OG");
                    GroupOccupationBE group = new GroupOccupationBE();
                    group.v_GroupOccupationId = newIdGroup;
                    group.v_Name = geso;
                    group.v_LocationId = newId;
                    group.i_InsertUserId = userId;
                    group.d_InsertDate = DateTime.Now;
                    group.i_IsDeleted = 0;
                    ctx.GroupOccupation.Add(group);
                    ctx.SaveChanges();
                }

                return newId;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private static string GetOrganizationByRuc(string ruc)
        {
            var objorg = ctx.Organization.Where(x => x.v_IdentificationNumber == ruc).FirstOrDefault();
            if (objorg == null)
            {
                
                return null;
            }
            var objLoc = ctx.Location.Where(x => x.v_OrganizationId == objorg.v_OrganizationId).FirstOrDefault();
            var locationId = "";
            if (objLoc != null)
            {
                locationId = objLoc.v_LocationId;
            }
            return objorg.v_OrganizationId + "|" + locationId;
        }

        public List<OrganizationBE> GetOrganizationByIds( List<string> ids )
        {
            var query = (from A in ctx.Organization where ids.Contains(A.v_OrganizationId) select A).ToList();

            return query;
        }

        public string GetGroupOcupation(string locationId, string gesoName)
        {
            var query = ctx.GroupOccupation.Where(x => x.v_LocationId == locationId && x.v_Name == gesoName).FirstOrDefault();
            if (query != null)
            {
                return query.v_GroupOccupationId;
            }
            return null;
        }

        public List<OrganizationWareHouse> GetWareHouses(string organizationId)
        {
            var query = (from A in ctx.NodeOrganizationLocationWarehouseProfile
                        join B in ctx.Warehouse on A.v_WarehouseId equals B.v_WarehouseId
                        where A.v_OrganizationId == organizationId
                select new OrganizationWareHouse
                {
                   WareHouseId = A.v_WarehouseId,
                   Name = B.v_Name
                }).ToList();

            return query;
        }


        public static string GetOrganizationIdByProtocolId(string protocolId)
        {
            var organizationId = (from a in ctx.Protocol where a.v_ProtocolId == protocolId && a.i_IsDeleted == 0 select a.v_WorkingOrganizationId).FirstOrDefault();
            return organizationId;
        }


        public List<string> SearchOrganizations(string name)
        {
            try
            {
                var query = (from a in ctx.Organization
                             join loc in ctx.Location on a.v_OrganizationId equals loc.v_OrganizationId into loc_join from loc in loc_join.DefaultIfEmpty()
                             where (a.v_Name.Contains(name) || a.v_IdentificationNumber.Contains(name)) && a.i_IsDeleted == (int)SiNo.No
                             select new
                             {
                                 value = a.v_Name + "|" + a.v_OrganizationId + "|" + a.v_IdentificationNumber + "|" + a.v_Address+ "|" + loc.v_LocationId + "|" + a.v_ContacName
                             }).ToList();

                if (query != null)
                {
                    return query.Select(p => p.value).ToList();
                }
                else
                {
                    return null;
                }
                
            }
            catch (Exception ex)
            {
                return null;
            }
            

        }
    }
}
