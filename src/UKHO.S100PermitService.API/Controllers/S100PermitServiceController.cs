﻿using Microsoft.AspNetCore.Mvc;
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
        public IActionResult GeneratePermit(string mid, string mkey, string hwid)
        {
            string permit = s100Service.GetUserPermitNumber(mid, mkey, hwid);

            return Json(permit);
        }

        [HttpGet("S100GetDecryptedHwdId")]
        public IActionResult GetDecryptedHwdId(string upn, string mkey)
        {
            string hwId = s100Service.GetDecryptedHwdId(upn, upn[40..], mkey);

            return Json(hwId);
        }

        [HttpGet("S100GetEncryptedDataKey")]
        public IActionResult GetEncryptedDataKey(string hwid)
        {
            /*** dummy data **/
            string dataKey = "123ABC";
            string fileName = "12345678";
            int edtn = 1;
            /*** dummy data **/

            
            string encryptedDataKey = s100Service.GetEncryptedDataKey(hwid, dataKey, fileName, edtn);

            return Json(encryptedDataKey);
        }
    }
}
