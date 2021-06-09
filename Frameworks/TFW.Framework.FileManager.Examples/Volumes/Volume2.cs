using elFinder.Net.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TFW.Framework.FileManager.Examples.Volumes
{
    public interface IVolume2 : IVolume
    {
    }

    public class Volume2 : Volume, IVolume2
    {
        public Volume2(IDriver driver, string rootDirectory, string url, string thumbUrl, char directorySeparatorChar = '\0') : base(driver, rootDirectory, url, thumbUrl, directorySeparatorChar)
        {
        }
    }
}
