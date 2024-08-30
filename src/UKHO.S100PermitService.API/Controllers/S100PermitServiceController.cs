using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using UKHO.S100PermitService.Common.Models;
using UKHO.S100PermitService.Common.Services;
using UKHO.S100PermitService.Common.Validators;

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
            S100UserPermit s100UserPermit = new() { MId = mid, MKey = mkey, HwId = hwid };
            UserPermitValidator validator = new();

            var result = validator.Validate(s100UserPermit);
            if (result.IsValid)
            {
                string permit = s100Service.GetUserPermitNumber(mid, mkey, hwid);
                return new JsonResult(permit);
            }
            return BadRequest(result.Errors);
        }

        [HttpGet]
        public IActionResult GetDecryptedHwdId(string upn, string mkey)
        {
            S100UserPermit s100UserPermit = new() { MKey = mkey, UserPermit = upn };
            DecryptUserPermitValidator validator = new();

            var result = validator.Validate(s100UserPermit);
            if (result.IsValid)
            {
                string hwId = s100Service.GetDecryptedHwdId(upn, upn[40..], mkey);

                return new JsonResult(hwId);
            }
            return BadRequest(result.Errors);
        }

        [HttpGet]
        public IActionResult GetEncryptedDataKey(string hwid, string dataKey)
        {
            /*** dummy data **/
            ////string dataKey = "1C81DFAB4053D04803FFDC87EF92FDD1";
            string fileName = "12345678";
            int edtn = 1;
            /*** dummy data **/


            string encryptedDataKey = s100Service.GetEncryptedDataKey(hwid, dataKey, fileName, edtn);

            return new JsonResult(encryptedDataKey);
        }

        [HttpGet]
        public IActionResult GetDecryptedDataKey(string hwid, string encryptedDataKey)
        {
            /*** dummy data **/
            ////string dataKey = "1C81DFAB4053D04803FFDC87EF92FDD1";
            string fileName = "12345678";
            int edtn = 1;
            /*** dummy data **/


            string decryptedDataKey = s100Service.GetDecryptedDataKey(hwid, encryptedDataKey, fileName, edtn);

            return new JsonResult(decryptedDataKey);
        }

        [HttpGet]
        public IActionResult GeneRateRandomHwId()
        {
            byte[] d = new byte[16];
            RandomNumberGenerator.Fill(d);
            return new JsonResult(BitConverter.ToString(d).Replace("-", ""));
        }
    }
}
