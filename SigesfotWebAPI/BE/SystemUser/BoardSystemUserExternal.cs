using BE.Common;
using BE.Protocol;
using BE.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.SystemUser
{
    public class BoardSystemUserExternal
    {
        public PersonDto EntityPerson { get; set; }
        public ProfessionalBE EntityProfessional { get; set; }
        public SystemUserDto EntitySystemUser { get; set; }
        public List<ProtocolSystemUserBE> ListEntityProtocolSystemUser { get; set; }
    }
}
