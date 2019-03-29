using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static BE.Common.Enumeratores;

namespace DAL.Warehouse
{
    public class WarehouseDal
    {
        public List<string> SearchProduct(string name)
        {
            using (var ctx = new DatabaseContext())
            {
                var query = (from a in ctx.Product
                    where a.v_Name.Contains(name) && a.i_IsDeleted == (int)SiNo.No
                    select new
                    {
                        value = a.v_Name + "|" + a.v_ProductId + "|" + a.v_AdditionalInformation
                    }).ToList();

                return query.Select(p => p.value).ToList();
            }
        }
    }
}
