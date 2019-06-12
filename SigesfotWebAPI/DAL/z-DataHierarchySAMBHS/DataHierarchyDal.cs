using BE.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BE.Common.Enumeratores;

namespace SAMBHSDAL.DataHierarchy
{
    public class DataHierarchyDal
    {
        DatabaseSAMBHSContext ctx = new DatabaseSAMBHSContext();
        public List<Dropdownlist> GetDatahierarchySAMBHSByGrupoId(int grupoId)
        {
            var isDeleted = (int)Enumeratores.SiNo.No;
            List<Dropdownlist> result = (from a in ctx.DataHierarchy
                                         where a.i_IsDeleted == isDeleted && a.i_GroupId == grupoId

                                         select new Dropdownlist
                                         {
                                             Id = a.i_ItemId.Value,
                                             Value = a.v_Value1
                                         }).ToList();
            return result;
        }

        public List<Dropdownlist> GetVendedor()
        {
            List<Dropdownlist> result = (from a in ctx.Vendedor
                                         where a.i_Eliminado == (int)SiNo.No && a.i_EsActivo == (int)SiNo.Si

                                         select new Dropdownlist
                                         {
                                             v_Id = a.v_IdVendedor,
                                             Value = a.v_NombreCompleto
                                         }).ToList();
            return result;
        }
    }
}
