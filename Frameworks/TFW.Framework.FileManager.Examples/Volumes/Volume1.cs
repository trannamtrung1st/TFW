using elFinder.Net.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TFW.Framework.FileManager.Examples.Volumes
{
    public interface IVolume1 : IVolume
    {
    }

    public class Volume1 : Volume, IVolume1
    {
        public Volume1(IDriver driver, string rootDirectory, string url, string thumbUrl, char directorySeparatorChar = '\0') : base(driver, rootDirectory, url, thumbUrl, directorySeparatorChar)
        {
        }
    }
}
