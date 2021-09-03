using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TFW.Framework.WebExamples.Filters;
using TFW.Framework.WebExamples.Helpers;

namespace TFW.Framework.WebExamples.Controllers
{

    [Route("api/streaming")]
    public class StreamingController : Controller
    {
        [HttpPost]
        [DisableFormValueModelBinding]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StreamToPhysicalStorage()
        {
            if (!FileHelper.IsMultipartContentType(Request.ContentType))
            {
                ModelState.AddModelError("File",
                    $"The request couldn't be processed (Error 1).");

                return BadRequest(ModelState);
            }

            var boundary = FileHelper.GetBoundary(
                MediaTypeHeaderValue.Parse(Request.ContentType), Startup.Settings.BoundaryLengthLimit);
            var reader = new MultipartReader(boundary, HttpContext.Request.Body);
            var section = await reader.ReadNextSectionAsync();

            while (section != null)
            {
                var hasContentDispositionHeader =
                    ContentDispositionHeaderValue.TryParse(
                        section.ContentDisposition, out var contentDisposition);

                if (hasContentDispositionHeader)
                {
                    // This check assumes that there's a file
                    // present without form data. If form data
                    // is present, this method immediately fails
                    // and returns the model error.
                    if (!FileHelper.HasFileContentDisposition(contentDisposition))
                    {
                        ModelState.AddModelError("File",
                            $"The request couldn't be processed (Error 2).");

                        return BadRequest(ModelState);
                    }
                    else
                    {
                        var cleanFileName = Path.GetFileName(contentDisposition.FileName.Value);
                        // Don't trust the file name sent by the client. To display
                        // the file name, HTML-encode the value.
                        var trustedFileNameForDisplay = HttpUtility.HtmlDecode(cleanFileName);
                        var trustedFileNameForFileStorage = Path.GetRandomFileName();

                        // **WARNING!**
                        // In the following example, the file is saved without
                        // scanning the file's contents. In most production
                        // scenarios, an anti-virus/anti-malware scanner API
                        // is used on the file before making the file available
                        // for download or for use by other systems. 
                        // For more information, see the topic that accompanies 
                        // this sample.

                        var streamedFileContent = await FileHelper.ProcessStreamedFile(
                            section, contentDisposition, ModelState,
                            default, Startup.Settings.FileSizeLimit);

                        if (!ModelState.IsValid)
                        {
                            return BadRequest(ModelState);
                        }

                        using (var targetStream = System.IO.File.Create(
                            Path.Combine(Startup.Settings.UploadFolder, trustedFileNameForFileStorage)))
                        {
                            await targetStream.WriteAsync(streamedFileContent);

                            //_logger.LogInformation(
                            //    "Uploaded file '{TrustedFileNameForDisplay}' saved to " +
                            //    "'{TargetFilePath}' as {TrustedFileNameForFileStorage}",
                            //    trustedFileNameForDisplay, _targetFilePath,
                            //    trustedFileNameForFileStorage);
                        }
                    }
                }

                // Drain any remaining section body that hasn't been consumed and
                // read the headers for the next section.
                section = await reader.ReadNextSectionAsync();
            }

            return Ok();
        }
    }
}
