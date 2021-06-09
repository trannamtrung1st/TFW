using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Framework.i18n
{
    public interface ITimeProvider
    {
        DateTime Now { get; }
        DateTime Today { get; }
        DateTimeOffset OffsetNow { get; }
        DateTimeKind Kind { get; }
        DateTime Normalize(DateTime dateTime);
    }
}
