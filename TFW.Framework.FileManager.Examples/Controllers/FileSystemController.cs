using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using elFinder.NetCore;
using elFinder.NetCore.Drivers.FileSystem;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace TFW.Framework.FileManager.Examples.Controllers
{
    [Route("el-finder/file-system")]
    public class FileSystemController : Controller
    {
        [Route("connector")]
        public async Task<IActionResult> Connector()
        {
            var connector = GetConnector();
            return await connector.ProcessAsync(Request);
        }

        [Route("thumb/{hash}")]
        public async Task<IActionResult> Thumbs(string hash)
        {
            var connector = GetConnector();
            return await connector.GetThumbnailAsync(HttpContext.Request, HttpContext.Response, hash);
        }

        private Connector GetConnector()
        {
            var driver = new FileSystemDriver();

            string absoluteUrl = UriHelper.BuildAbsolute(Request.Scheme, Request.Host);
            var uri = new Uri(absoluteUrl);

            var root = new RootVolume(
                Startup.MapPath("~/upload"),
                $"{uri.Scheme}://{uri.Authority}/upload/",
                $"{uri.Scheme}://{uri.Authority}/el-finder/file-system/thumb/")
            {
                StartDirectory = "ReadWrite/Prohibited/test\\asd/",
                //IsReadOnly = !User.IsInRole("Administrators")
                IsReadOnly = false, // Can be readonly according to user's membership permission
                IsLocked = false, // If locked, files and directories cannot be deleted, renamed or moved
                Alias = "Files", // Beautiful name given to the root/home folder
                //MaxUploadSizeInKb = 2048, // Limit imposed to user uploaded file <= 2048 KB
                //DefaultAttribute = new ItemAttribute
                //{
                //    Locked = true,
                //    Read = false,
                //    Write = false
                //},
                ItemAttributes = new HashSet<SpecificItemAttribute>(),
                ThumbnailSize = 256
            };

            root.AddItemAttribute(Startup.MapPath("~/upload/ReadWrite/Prohibited/test\\asd/"), new ItemAttribute()
            {
                Write = false,
                Locked = true,
                Read = false
            });

            driver.AddRoot(root);

            return new Connector(driver)
            {
                // This allows support for the "onlyMimes" option on the client.
                MimeDetect = MimeDetectOption.Internal
            };
        }
    }
}