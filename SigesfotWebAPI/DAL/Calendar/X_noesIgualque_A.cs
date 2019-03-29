using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE.Common;

namespace DAL.Calendar
{
   public class X_noesIgualque_A : OperatorX
    {
        public override int IsRequired(int pacientAge, int analyzeAge, int pacientGender, int analyzeGender)
        {
            if ((int)Enumeratores.GenderConditional.Ambos == analyzeGender)
            {
                if (pacientAge != analyzeAge)
                    return (int)Enumeratores.SiNo.Si;

                return (int)Enumeratores.SiNo.No;
            }

            if (pacientAge != analyzeAge && pacientGender == analyzeGender)
                return (int)Enumeratores.SiNo.Si;

            return (int)Enumeratores.SiNo.No;
        }
    }
}
