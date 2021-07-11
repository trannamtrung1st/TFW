using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;


namespace TAuth.ResourceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("WorkerOnly")]
    public class BackgroundController : ControllerBase
    {

        [HttpGet]
        public object Get()
        {
            return new
            {
                Status = "Ok",
                Time = DateTimeOffset.UtcNow
            };
        }
    }
}
