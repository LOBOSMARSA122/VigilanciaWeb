using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE.Common;

namespace DAL.Calendar
{
    public class OperatorX
    {
        public Enumeratores.Operator2Values value { get; set; }

        public virtual int IsRequired(int pacientAge, int analyzeAge, int pacientGender, int analyzeGender)
        {
            return 0;
        }
    }
}
