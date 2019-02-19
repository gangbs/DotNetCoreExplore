using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DotNetCore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        public ActionResult<object> Get()
        {
            string sessionId = HttpContext.Session.Id;
            HttpContext.Session.SetString("id", sessionId);
            HttpContext.Session.SetString("yyg", "123");


            return Ok(new { id = sessionId });
        }
    }
}