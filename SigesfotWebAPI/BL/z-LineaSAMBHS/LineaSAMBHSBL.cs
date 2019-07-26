using BE.Z_CommonSAMBHS;
using DAL.z_LineasSAMBHS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.z_LineaSAMBHS
{
    public class LineaSAMBHSBL
    {
        public static List<LineaBE> GetAllLineas()
        {
            return LineaSAMBHSDal.GetAllLineas();
        }
    }
}
