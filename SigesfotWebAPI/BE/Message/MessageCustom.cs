using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Message
{
    public class MessageCustom
    {
        public bool Error { get; set; }
        public string Id { get; set; }
        public string Message { get; set; }
        public int Status { get; set; }
    }
}
