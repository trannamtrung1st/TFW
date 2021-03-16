using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace TFW.Framework.Common.Helpers
{
    public static class StreamHelper
    {
        public static Task<string> ReadAsStringAsync(this Stream stream)
        {
            return new StreamReader(stream).ReadToEndAsync();
        }
    }
}
