using Microsoft.AspNetCore.Mvc;
using UKHO.S100PermitService.Common.Services;

namespace UKHO.S100PermitService.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class S100PermitServiceController : Controller
    {
        private readonly S100Service s100Service;
        public S100PermitServiceController()
        {
            this.s100Service = new S100Service();
        }

        [HttpGet]
        public IActionResult GeneratePermit(string mid, string mkey, string hwid)
        {
            string permit = s100Service.GetUserPermitNumber(mid, mkey, hwid);

            return new JsonResult(permit);
        }

        [HttpGet]
        public IActionResult GetDecryptedHwdId(string upn, string mkey)
        {
            string hwId = s100Service.GetDecryptedHwdId(upn, upn[40..], mkey);

            return new JsonResult(hwId);
        }

        [HttpGet]
        public IActionResult GetEncryptedDataKey(string hwid)
        {
            /*** dummy data **/
            string dataKey = "123ABC";
            string fileName = "12345678";
            int edtn = 1;
            /*** dummy data **/


            string encryptedDataKey = s100Service.GetEncryptedDataKey(hwid, dataKey, fileName, edtn);

            return new JsonResult(encryptedDataKey);
        }
    }
}
