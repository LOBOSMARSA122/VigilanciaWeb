using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE.Diagnostic;
using DAL.Diagnostic;

namespace BL
{
    public class DiagnosticBl
    {
        public List<DiagnosticCustom> GetDiagnosticByServiceId(string serviceId)
        {
           return new DiagnosticDal().GetDiagnosticsByServiceId(serviceId);
        }
        
    }
}
