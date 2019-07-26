using SigesoftWeb.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SigesoftWeb.Utils
{
    public class ClientSession
    {
        public int SystemUserId { get; set; }
        public int EstablecimientoPredeterminado { get; set; }
        public string PersonId { get; set; }       
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string RoleId { get; set; }
        public byte[] PersonImage { get; set; }
        public string SystemUserByOrganizationId { get; set; }
        public int NodeId { get; set; }
        public string RucEmpresa { get; set; }
        public string TelephoneNumber { get; set; }
        public string KeySMS { get; set; }
        public List<Permission> Permissions { get; set; }
        public List<OrganizationSystemUser> Organizations { get; set; }
        public List<Option> Options { get; set; }
    }
}