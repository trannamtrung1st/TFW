﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;

namespace TFW.Framework.PollyWrapper.Examples.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DefectsController : ControllerBase
    {
        [HttpGet("timeout")]
        public IActionResult TimeOut()
        {
            Thread.Sleep(10000);
            return Ok("Success");
        }

        [HttpGet("always-fail")]
        public IActionResult AlwaysFail()
        {
            return StatusCode(500);
        }

        [HttpGet("need-token")]
        public IActionResult NeedToken([FromQuery] string token)
        {
            if (token == "right-token")
            {
                if (DateTimeOffset.UtcNow.Ticks % 2 == 0)
                    return Ok("It's okay");

                return StatusCode(500);
            }

            return Unauthorized();
        }
    }
}
