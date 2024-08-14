using Microsoft.AspNetCore.Mvc;

namespace UKHO.S100PermitService.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class S100PermitServiceController : Controller
    {
        public S100PermitServiceController()
        {
        }

        [HttpGet(Name = "S100PermitGenerate")]
        public IActionResult GeneratePermit(string upn, string mkey)
        {
            //temp code
            return new JsonResult(upn+mkey);
        }
    }
}
