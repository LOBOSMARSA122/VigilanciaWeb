using System.Web.Http;
using BE.Common;
using BE.Worker;
using BL.Subscribe;
using Newtonsoft.Json;

namespace SigesoftWebAPI.Controllers
{
    public class SubscribeController : ApiController
    {
        private readonly SubscribeBl oSubscribeBl = new SubscribeBl();

        [HttpPost]
        public IHttpActionResult Subscribe(MultiDataModel model)
        {
            if (string.IsNullOrWhiteSpace(model.String1))
                return BadRequest("Información Inválida");
            var data = JsonConvert.DeserializeObject<VapidBe>(model.String1);

            oSubscribeBl.Subscribe(1, data.Subs, data.PersonId);
            return Ok();
        }

        [HttpPost]
        public IHttpActionResult UnSubscribe(MultiDataModel model)
        {
            if (string.IsNullOrWhiteSpace(model.String1))
                return BadRequest("Información Inválida");
            var data = JsonConvert.DeserializeObject<VapidBe>(model.String1);

            var result = oSubscribeBl.UnSubscribe(data.PersonId);
            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult GetSubscriptions(string personId)
        {
            var result = oSubscribeBl.GetSubscriptions(personId);
            return Ok(result);
        }

    }
}
