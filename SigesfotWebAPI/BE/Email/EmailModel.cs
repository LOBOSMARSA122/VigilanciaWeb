using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BL.Email
{
    public class EmailModel
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string ServiceOrderId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int TypeEmail { get; set; }
    }
}
