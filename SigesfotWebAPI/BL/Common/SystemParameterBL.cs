using BE.Common;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Common
{
    public class SystemParameterBL
    {
        private DatabaseContext ctx = new DatabaseContext();

        public List<Dropdownlist> GetParametroByGrupoId(int grupoId)
        {
            var isDeleted = (int)Enumeratores.SiNo.No;
            List<Dropdownlist> result = (from a in ctx.SystemParameter
                                         where a.i_IsDeleted == isDeleted && a.i_GroupId == grupoId
                                         orderby a.i_Sort ascending
                                         select new Dropdownlist
                                         {
                                             Id = a.i_ParameterId,
                                             Value = a.v_Value1
                                         }).ToList();
            return result;
        }
    }
}
