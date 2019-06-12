using BE.Common;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Common
{
    public class DataHierarchyBL
    {
        private DatabaseContext ctx = new DatabaseContext();

        #region Bussines Logic
        public List<Dropdownlist> GetDatahierarchyByGrupoId(int grupoId)
        {
            var isDeleted = (int)Enumeratores.SiNo.No;
            List<Dropdownlist> result = (from a in ctx.DataHierarchy
                                         where a.i_IsDeleted == isDeleted && a.i_GroupId == grupoId
                                         
                                         select new Dropdownlist
                                         {
                                             Id = a.i_ItemId,
                                             Value = a.v_Value1
                                         }).ToList();
            return result;
        }

        public List<Dropdownlist> GetDistritos(string name)
        {
            var isDeleted = (int)Enumeratores.SiNo.No;
            List<Dropdownlist> result = (from a in ctx.DataHierarchy
                                         where a.i_IsDeleted == isDeleted && a.i_GroupId == 113 && a.v_Value1 == name
                                         orderby a.i_ParentItemId descending
                                         select new Dropdownlist
                                         {
                                             Id = a.i_ParentItemId.Value,
                                             Value = a.v_Value1
                                         }).ToList();
            return result;
        }

        public List<Dropdownlist> GetProvincia(int idDistrito)
        {
            var isDeleted = (int)Enumeratores.SiNo.No;
            List<Dropdownlist> result = (from a in ctx.DataHierarchy
                                         where a.i_IsDeleted == isDeleted && a.i_GroupId == 113 && a.i_ItemId == idDistrito
                                         orderby a.i_ParentItemId descending
                                         select new Dropdownlist
                                         {
                                             Id = a.i_ItemId,
                                             Value = a.v_Value1,
                                             Value2 = a.i_ParentItemId.Value
                                         }).ToList();
            return result;
        }

        public List<Dropdownlist> GetDepartamento(int idProvincia)
        {
            var isDeleted = (int)Enumeratores.SiNo.No;
            List<Dropdownlist> result = (from a in ctx.DataHierarchy
                                         where a.i_IsDeleted == isDeleted && a.i_GroupId == 113 && a.i_ItemId == idProvincia

                                         select new Dropdownlist
                                         {
                                             Id = a.i_ItemId,
                                             Value = a.v_Value1
                                         }).ToList();
            return result;
        }
        #endregion
    }
}
