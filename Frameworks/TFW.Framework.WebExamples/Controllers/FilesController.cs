using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TFW.Framework.WebExamples.Entities;
using TFW.Framework.WebExamples.Models;

namespace TFW.Framework.WebExamples.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly DataContext _dbContext;

        public FilesController(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost("buffered-upload")]
        public async Task<IActionResult> BufferedUploadAsync([FromForm] IFormFileCollection files)
        {
            long size = files.Sum(f => f.Length);

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    // Process uploaded files
                    // Don't rely on or trust the FileName property without validation.

                    var uploadFileName = formFile.FileName;
                    var cleanFileName = Path.GetFileName(uploadFileName);
                    var filePath = Path.Combine(Startup.Settings.UploadFolder,
                        $"{Path.GetRandomFileName()}_{DateTimeOffset.UtcNow.Ticks}_{cleanFileName}");

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }

            return Ok(new FileUploadResponse
            {
                Count = files.Count,
                Size = size
            });
        }

        [HttpPost("database-upload")]
        public async Task<IActionResult> DatabaseUploadAsync([FromForm] IFormFileCollection files)
        {
            double MB2 = Math.Pow(1024, 2) * 2;

            if (files.Any(f => f.Length > MB2))
                return Problem("Files too large", title: "Files too large", statusCode: StatusCodes.Status400BadRequest);

            long size = files.Sum(f => f.Length);

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    // Process uploaded files
                    // Don't rely on or trust the FileName property without validation.

                    var uploadFileName = formFile.FileName;
                    var cleanFileName = Path.GetFileName(uploadFileName);

                    using (var stream = new MemoryStream())
                    {
                        await formFile.CopyToAsync(stream);

                        var file = new FileEntity()
                        {
                            Content = stream.ToArray(),
                            DisplayName = cleanFileName
                        };

                        _dbContext.Files.Add(file);

                        await _dbContext.SaveChangesAsync();
                    }
                }
            }

            return Ok(new FileUploadResponse
            {
                Count = files.Count,
                Size = size
            });
        }
    }
}
