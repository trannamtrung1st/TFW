using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace TFW.Framework.Common.Helpers
{
    public static class ProcessHelper
    {
        public static Process Build(this Process process,
            string fileName, string arguments = "", string workingDir = "",
            ProcessWindowStyle windowStyle = ProcessWindowStyle.Hidden)
        {
            process.StartInfo = new ProcessStartInfo()
                .WindowStyle(windowStyle)
                .WorkingDirectory(workingDir)
                .FileName(fileName)
                .Arguments(arguments);

            return process;
        }

        public static ProcessStartInfo WindowStyle(this ProcessStartInfo startInfo, ProcessWindowStyle windowStyle)
        {
            startInfo.WindowStyle = windowStyle;
            return startInfo;
        }

        public static ProcessStartInfo Arguments(this ProcessStartInfo startInfo, string arguments)
        {
            startInfo.Arguments = arguments;
            return startInfo;
        }

        public static ProcessStartInfo FileName(this ProcessStartInfo startInfo, string fileName)
        {
            startInfo.FileName = fileName;
            return startInfo;
        }

        public static ProcessStartInfo WorkingDirectory(this ProcessStartInfo startInfo, string dir)
        {
            startInfo.WorkingDirectory = dir;
            return startInfo;
        }

    }
}
