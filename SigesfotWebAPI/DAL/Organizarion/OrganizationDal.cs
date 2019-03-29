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
