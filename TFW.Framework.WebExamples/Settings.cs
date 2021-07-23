using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TFW.Framework.WebExamples
{
    public class Settings
    {
        public string UploadFolder { get; set; }
        public long FileSizeLimit { get; set; }
        public long BoundaryLengthLimit { get; set; }
    }
}
