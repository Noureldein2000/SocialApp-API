using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dating_Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ValsuesController : ControllerBase
    {
        [Authorize(Roles ="Admin, Moderator")]
        [HttpGet]
        public IActionResult GetValues()
        {
            return Ok();
        }
    }
}
