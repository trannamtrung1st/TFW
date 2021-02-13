using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace TFW.Framework.Common
{
    public interface ITimeZoneResolver
    {
        public TimeZoneInfo Current { get; }
    }

    public class DefaultTimeZoneResolver : ITimeZoneResolver
    {
        private readonly IDictionary<string, TimeZoneInfo> _timeZoneMap;
        private Func<CultureInfo, string> _getCurrentCultureFunc
            = ((cultureInfo) => cultureInfo.TwoLetterISOLanguageName);

        public DefaultTimeZoneResolver(IDictionary<string, string> timeZoneMap,
            Func<CultureInfo, string> getCurrentCultureFunc = null)
        {
            if (timeZoneMap == null || !timeZoneMap.Any())
                throw new ArgumentException(message: "Invalid time zone map", nameof(timeZoneMap));
            _timeZoneMap = timeZoneMap.ToDictionary(o => o.Key, o =>
                TimeZoneInfo.FindSystemTimeZoneById(o.Value));
            if (getCurrentCultureFunc != null)
                _getCurrentCultureFunc = getCurrentCultureFunc;
        }

        public DefaultTimeZoneResolver(IDictionary<string, TimeZoneInfo> timeZoneMap,
            Func<CultureInfo, string> getCurrentCultureFunc = null)
        {
            if (timeZoneMap == null || !timeZoneMap.Any())
                throw new ArgumentException(message: "Invalid time zone map", nameof(timeZoneMap));
            _timeZoneMap = timeZoneMap;
            if (getCurrentCultureFunc != null)
                _getCurrentCultureFunc = getCurrentCultureFunc;
        }

        public TimeZoneInfo Current
        {
            get
            {
                var cultureStr = _getCurrentCultureFunc(CultureInfo.CurrentCulture);
                if (_timeZoneMap.ContainsKey(cultureStr))
                    return _timeZoneMap[cultureStr];
                return _timeZoneMap.First().Value;
            }
        }
    }
}
