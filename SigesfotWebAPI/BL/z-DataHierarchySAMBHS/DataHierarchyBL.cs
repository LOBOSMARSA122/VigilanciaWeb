using BE.Common;
using SAMBHSDAL.DataHierarchy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAMBHSBL.DataHierarchy
{
    public class DataHierarchyBL
    {
        public List<Dropdownlist> GetDatahierarchySAMBHSByGrupoId(int grupoId)
        {
            return new DataHierarchyDal().GetDatahierarchySAMBHSByGrupoId(grupoId);
        }

        public List<Dropdownlist> GetVendedor()
        {
            return new DataHierarchyDal().GetVendedor();
        }
    }
}
