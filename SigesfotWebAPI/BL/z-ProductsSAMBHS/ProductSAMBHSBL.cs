using BE.Z_SAMBHSCUSTOM.Productos;
using DAL.z_ProductsSAMBHS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.z_ProductsSAMBHS
{
    public class ProductSAMBHSBL
    {
        public BoardProductsSAMBHS GetProductsSAMBHS(BoardProductsSAMBHS data)
        {
            return new ProductSAMBHSDal().GetProductsSAMBHS_PV(data);
        }
    }
}
