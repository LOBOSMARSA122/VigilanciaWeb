using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Warehouse;

namespace BL.WareHouse
{
   public class WarehouseBl
    {
        public List<string> SearchProduct(string name)
        {
            return new WarehouseDal().SearchProduct(name);
        }
    }
}
