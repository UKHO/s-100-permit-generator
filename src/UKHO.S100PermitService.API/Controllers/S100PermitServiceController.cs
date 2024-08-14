using Microsoft.AspNetCore.Mvc;
using UKHO.S100PermitService.Common;

namespace UKHO.S100PermitService.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class S100PermitServiceController : Controller
    {
        private readonly S100Service s100Service;
        public S100PermitServiceController()
        {
            this.s100Service = new S100Service();
        }

        [HttpGet("S100GeneratePermit")]
        public IActionResult GeneratePermit(string upn, string mkey)
        {
            string mid = upn[40..];
            string hwId = s100Service.GetDecryptedHwdId(upn, mid, mkey);
            string permit = s100Service.GetUserPermitNumber(mid, mkey, hwId);

            return Json(permit);
        }

        [HttpGet("S100GetDecryptedHwdId")]
        public IActionResult GetDecryptedHwdId(string upn, string mkey)
        {
            string hwId = s100Service.GetDecryptedHwdId(upn, upn[40..], mkey);

            return Json(hwId);
        }

        [HttpGet("S100GetEncryptedDataKey")]
        public IActionResult GetEncryptedDataKey(string upn, string mkey)
        {
            /*** dummy data **/
            string dataKey = "123";
            string fileName = "test";
            int edtn = 1;
            /*** dummy data **/

            string hwId = s100Service.GetDecryptedHwdId(upn, upn[40..], mkey);
            
            string encryptedDataKey = s100Service.GetEncryptedDataKey(hwId, dataKey, fileName, edtn);

            return Json(encryptedDataKey);
        }
    }
}
