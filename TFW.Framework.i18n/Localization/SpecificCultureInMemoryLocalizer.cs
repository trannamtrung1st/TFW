using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace TFW.Framework.i18n.Localization
{
    internal class SpecificCultureInMemoryLocalizer<T> : IStringLocalizer, IStringLocalizer<T>
    {
        public CultureInfo CultureInfo { get; }
        private readonly IDictionary<string, string> _resources;

        public SpecificCultureInMemoryLocalizer(CultureInfo cultureInfo, IDictionary<string, string> resources)
        {
            CultureInfo = cultureInfo;
            _resources = resources;
        }

        public LocalizedString this[string name] => GetValue(name);

        public LocalizedString this[string name, params object[] arguments] => GetValue(name, arguments);

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            return _resources.Select(kvp => new LocalizedString(kvp.Key, kvp.Value));
        }

        public IStringLocalizer WithCulture(CultureInfo culture)
        {
            throw new InvalidOperationException();
        }

        private LocalizedString GetValue(string name, params object[] args)
        {
            string value;
            var notFound = !_resources.TryGetValue(name, out value);

            if (notFound) value = name;

            if (args.Length > 0)
                value = string.Format(CultureInfo.CurrentUICulture, value, args);

            return new LocalizedString(name, value, notFound);
        }
    }

}
