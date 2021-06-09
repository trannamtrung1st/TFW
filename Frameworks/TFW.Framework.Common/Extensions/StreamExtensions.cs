using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace TFW.Framework.Common.Extensions
{
    public static class StreamExtensions
    {
        public static Task<string> ReadAsStringAsync(this Stream stream)
        {
            return new StreamReader(stream).ReadToEndAsync();
        }
    }
}
