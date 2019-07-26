using BE.Z_CommonSAMBHS;
using SAMBHSDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.z_LineasSAMBHS
{
    public class LineaSAMBHSDal
    {
        public static DatabaseSAMBHSContext ctx = new DatabaseSAMBHSContext();
        public static List<LineaBE> GetAllLineas()
        {
            try
            {
                var list = ctx.Linea.Where(x => x.i_Eliminado == 0).ToList();
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
