using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace TFW.Framework.WebExamples.Controllers
{
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [Route("/error")]
        public IActionResult HandleError([FromServices] IWebHostEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                var context = HttpContext.Features.Get<IExceptionHandlerFeature>();

                if (context.Error == null) return BadRequest();

                return Problem(
                    detail: context.Error.StackTrace,
                    title: context.Error.Message);
            }

            return Problem();
        }
    }
}
