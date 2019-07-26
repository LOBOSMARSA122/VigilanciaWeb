using BE.Common;
using BE.Organization;
using BL.Organization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SigesoftWebAPI.Controllers.Organization
{
    public class OrganizationController : ApiController
    {
        [HttpGet]
        public IHttpActionResult SearchOrganizations(string value)
        {
            var result = OrganizationBL.SearchOrganizations(value);
            return Ok(result);
        }
        [HttpPost]
        public IHttpActionResult SaveOrganization(MultiDataModel multi)
        {
            OrganizationBE data = JsonConvert.DeserializeObject<OrganizationBE>(multi.String1);
            var result = OrganizationBL.SaveOrganization(data, multi.Int1, multi.Int2);

            return Ok(result);

        }
    }
}
