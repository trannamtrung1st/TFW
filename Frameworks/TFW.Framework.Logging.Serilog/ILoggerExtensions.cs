using Serilog;
using System.Runtime.CompilerServices;

namespace TFW.Framework.Logging.Serilog.Helpers
{
    public static class ILoggerExtensions
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
    }
}
