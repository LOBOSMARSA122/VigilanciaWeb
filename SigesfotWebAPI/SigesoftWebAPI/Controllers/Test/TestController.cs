using BL.Test;
using System.Threading.Tasks;
using System.Web.Http;

namespace SigesoftWebAPI.Controllers.Test
{
    public class TestController : ApiController
    {
        private TestBl oTestBl = new TestBl();
        [HttpGet]
        public async Task<IHttpActionResult> GetTest()
        {
            var result = 0;
            return await Task.Run(() => {
                result = oTestBl.Test();
                return Ok(result);
            });
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetTest2()
        {
            var result = 0;
            return await Task.Run(() => {
                result = oTestBl.Test2();
                return Ok(result);
            });
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetTest3(int nroPage)
        {
            return await Task.Run(() => {
               var result = oTestBl.GetTest3(nroPage);
                return Ok(result);
            });
        }

        [HttpGet]
        public IHttpActionResult Concurrence(int count)
        {
            oTestBl.AddTestConcurrence(count);
            return Ok("OK");
        }
    }
}
