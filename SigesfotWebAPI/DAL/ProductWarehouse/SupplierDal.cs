using BE.ProductWarehouse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ProductWarehouse
{
    public class SupplierDal
    {
        public BoardSupplier GetSuppliers(BoardSupplier data)
        {
            try
            {

                string filterSupplierRazonSocial = string.IsNullOrWhiteSpace(data.RazonSocial) ? "" : data.RazonSocial;
                string filterSupplierRuc = string.IsNullOrWhiteSpace(data.RazonSocial) ? "" : data.RazonSocial;
                int skip = (data.Index - 1) * data.Take;
                DatabaseContext dbContext = new DatabaseContext();
                var query = (from A in dbContext.Supplier
                             join C in dbContext.DataHierarchy on A.i_SectorTypeId equals C.i_ItemId
                             join J1 in dbContext.SystemUser on new { i_InsertUserId = A.i_InsertUserId }
                                                             equals new { i_InsertUserId = J1.i_SystemUserId } into J1_join
                             from J1 in J1_join.DefaultIfEmpty()

                             join J2 in dbContext.SystemUser on new { i_UpdateUserId = A.i_UpdateUserId }
                                                             equals new { i_UpdateUserId = J2.i_SystemUserId } into J2_join
                             from J2 in J2_join.DefaultIfEmpty()
                             where C.i_GroupId == 104 && ( data.SectorId == -1 || A.i_SectorTypeId == data.SectorId) 
                             &&  (A.v_Name.Contains(filterSupplierRazonSocial) || A.v_IdentificationNumber.Contains(filterSupplierRuc))
                             select new SupplierList
                             {
                                 SupplierId = A.v_SupplierId,
                                 SectorTypeId = (int)A.i_SectorTypeId,
                                 SectorTypeIdName = C.v_Value1,
                                 IdentificationNumber = A.v_IdentificationNumber,
                                 Name = A.v_Name,
                                 Address = A.v_Address,
                                 PhoneNumber = A.v_PhoneNumber,
                                 Mail = A.v_Mail,
                                 CreationUser = J1.v_UserName,
                                 UpdateUser = J2.v_UserName,
                                 CreationDate = A.d_InsertDate,
                                 UpdateDate = A.d_UpdateDate,
                                 IsDeleted = A.i_IsDeleted
                             }).ToList();
                if (data.Take > 0)
                    query = query.Skip(skip).Take(data.Take).ToList();


                data.TotalRecords = query.ToList().Count;
                data.List = query.OrderBy(a => a.Name).ToList();




                return data;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
