using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Common
{
    public class OperationResult
    {
        public int Success { get; set; }
        public string ExceptionMessage { get; set; }
        public string ErrorMessage { get; set; }
        public int ErrorCode { get; set; }
        public string AdditionalInformation { get; set; }
        public string ReturnValue { get; set; }
        public string OriginalExceptionMessage { get; set; }
    }
}
