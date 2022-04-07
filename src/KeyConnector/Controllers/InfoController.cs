using System;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace Bit.KeyConnector.Controllers
{
    public class InfoController : Controller
    {
        [HttpGet("~/alive")]
        [HttpGet("~/now")]
        public DateTime GetAlive()
        {
            return DateTime.UtcNow;
        }

        [HttpGet("~/version")]
        public JsonResult GetVersion()
        {
            return Json(Assembly.GetEntryAssembly()
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                .InformationalVersion);
        }
    }
}
