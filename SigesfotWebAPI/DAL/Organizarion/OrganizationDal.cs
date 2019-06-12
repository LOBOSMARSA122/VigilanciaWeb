using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using BE.Organization;
using BE.Security;
using BE.Warehouse;

namespace DAL.Organizarion
{
   public  class OrganizationDal
    {
        private static DatabaseContext ctx = new DatabaseContext();
        
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

    }
}
