using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using elFinder.Net.AspNetCore.Extensions;
using elFinder.Net.AspNetCore.Helper;
using elFinder.Net.Core;
using Microsoft.AspNetCore.Mvc;

namespace TFW.Framework.FileManager.Examples.Controllers
{
    [Route("api/files")]
    public class FilesController : Controller
    {
        private readonly IConnector _connector;
        private readonly IEnumerable<IVolume> _volumes;

        public FilesController(IConnector connector,
            IEnumerable<IVolume> volumes)
        {
            _connector = connector;
            _volumes = volumes;
        }

        [Route("connector")]
        public async Task<IActionResult> Connector()
        {
            SetupConnector();
            var cmd = ConnectorHelper.ParseCommand(Request);
            var conResult = await _connector.ProcessAsync(cmd);
            var actionResult = conResult.ToActionResult();
            return actionResult;
        }

        [Route("thumb/{target}")]
        public async Task<IActionResult> Thumb(string target)
        {
            SetupConnector();
            var thumb = await _connector.GetThumbAsync(target);
            var actionResult = ConnectorHelper.GetThumbResult(thumb);
            return actionResult;
        }

        private void SetupConnector()
        {
            foreach (var volume in _volumes)
            {
                volume.ItemAttributes = new HashSet<SpecificItemAttribute>()
                {
                    new SpecificItemAttribute($"{volume.RootDirectory}{volume.DirectorySeparatorChar}init")
                    {
                        Locked = true, Read = true, Write = false
                    },
                    new SpecificItemAttribute($"{volume.RootDirectory}{volume.DirectorySeparatorChar}halo.txt")
                    {
                        Locked = true, Read = true, Write = false
                    }
                };
                _connector.AddVolume(volume);
                volume.Driver.AddVolume(volume);
            }
        }
    }
}