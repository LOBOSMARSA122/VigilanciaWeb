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
                                         orderby a.i_Sort ascending
                                         select new Dropdownlist
                                         {
                                             Id = a.i_ItemId,
                                             Value = a.v_Value1
                                         }).OrderBy(a => a.Value).ToList();
            return result;
        }
        #endregion
    }
}
