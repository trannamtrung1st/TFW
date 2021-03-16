﻿using Serilog;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace TFW.Framework.Logging.Serilog.Helpers
{
    public static class LoggerHelper
    {
        public static ILogger CallerContext(this ILogger logger,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string fileName = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            return logger.ForContext(Properties.CallerMemberName, memberName)
                .ForContext(Properties.CallerFilePath, fileName)
                .ForContext(Properties.CallerLineNumber, lineNumber);
        }

        public static (string, object)[] CallerContext([CallerMemberName] string memberName = "",
            [CallerFilePath] string fileName = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            return new (string, object)[]
            {
                (Properties.CallerMemberName, memberName),
                (Properties.CallerFilePath, fileName),
                (Properties.CallerLineNumber, lineNumber),
            };
        }
    }
}