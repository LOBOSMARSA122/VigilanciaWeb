using BE.ProductWarehouse;
using DAL.ProductWarehouse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.ProductWarehouse
{
    public class SupplierBl
    {
        SupplierDal _SupplierDal = new SupplierDal();
        public BoardSupplier GetdataSuppliers(BoardSupplier data)
        {
            return _SupplierDal.GetSuppliers(data);
        }
    }
}
