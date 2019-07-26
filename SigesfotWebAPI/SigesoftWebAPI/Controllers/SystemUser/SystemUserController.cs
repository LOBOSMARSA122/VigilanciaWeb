using BE.Common;
using BE.SystemUser;
using BL.SystemUser;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SigesoftWebAPI.Controllers.SystemUser
{
    public class SystemUserController : ApiController
    {
        [HttpPost]
        public IHttpActionResult NewSystemUserExternal(MultiDataModel dataModel)
        {
            BoardSystemUserExternal data = JsonConvert.DeserializeObject<BoardSystemUserExternal>(dataModel.String1);
            var result = SystemUserBL.AddSystemUserExternal(data.EntityPerson, data.EntityProfessional, data.EntitySystemUser, data.ListEntityProtocolSystemUser , dataModel.Int1, dataModel.Int2);
            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult GetSystemUserById(int systemUserId)
        {
            var result = SystemUserBL.GetSystemUserById(systemUserId);
            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult DeletedSystemUser(int systemUserId)
        {
            var result = SystemUserBL.DeletedSystemUser(systemUserId);
            return Ok(result);
        }
    }
}
