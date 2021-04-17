using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using TFW.Framework.i18n.Options;

namespace TFW.Framework.i18n.Localization
{
    public class InMemoryLocalizer<T> : IStringLocalizer, IStringLocalizer<T>
    {
        private readonly InMemoryLocalizerOptions _options;
        private readonly Type _contextType;

        public InMemoryLocalizer(IOptions<InMemoryLocalizerOptions> options)
        {
            _contextType = typeof(T);
            _options = options.Value;
        }

        public LocalizedString this[string name] => GetValue(name);

        public LocalizedString this[string name, params object[] arguments] => GetValue(name, arguments);

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            var currentUiCulture = CultureInfo.CurrentUICulture.Name;
            var currentUiLang = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;

            IEnumerable<LocalizedString> allStrings = new List<LocalizedString>();

            if (_options.Resources.ContainsKey(_contextType))
            {
                if (_options.Resources[_contextType].ContainsKey(currentUiCulture))
                    allStrings = allStrings.Concat(
                        _options.Resources[_contextType][currentUiCulture]
                            .Select(kvp => new LocalizedString(kvp.Key, kvp.Value)).ToArray());

                if (includeParentCultures && currentUiCulture != currentUiLang
                    && _options.Resources[_contextType].ContainsKey(currentUiLang))
                    allStrings = allStrings.Concat(
                        _options.Resources[_contextType][currentUiLang]
                            .Select(kvp => new LocalizedString(kvp.Key, kvp.Value)).ToArray());
            }

            return allStrings;
        }

        public IStringLocalizer WithCulture(CultureInfo culture)
        {
            IDictionary<string, IDictionary<string, string>> typedResources;
            IDictionary<string, string> resources = null;

            if (_options.Resources.TryGetValue(_contextType, out typedResources))
            {
                if (!typedResources.TryGetValue(culture.Name, out resources)
                    && !typedResources.TryGetValue(culture.TwoLetterISOLanguageName, out resources))
                    typedResources.TryGetValue(string.Empty, out resources);
            }

            if (resources == null) return null;

            return new SpecificCultureInMemoryLocalizer<T>(culture, resources);
        }

        private LocalizedString GetValue(string name, params object[] args)
        {
            var currentUiCulture = CultureInfo.CurrentUICulture.Name;
            var currentUiLang = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            string value = null; bool found = false;
            IDictionary<string, IDictionary<string, string>> typedResources;
            IDictionary<string, string> resources;

            if (_options.Resources.TryGetValue(_contextType, out typedResources))
            {
                found = typedResources.TryGetValue(currentUiCulture, out resources)
                    && resources.TryGetValue(name, out value);

                if (!found && currentUiCulture != currentUiLang)
                    found = typedResources.TryGetValue(currentUiLang, out resources)
                        && resources.TryGetValue(name, out value);

                if (!found) found = typedResources.TryGetValue(string.Empty, out resources)
                        && resources.TryGetValue(name, out value);
            }

            if (!found) value = name;

            if (args.Length > 0)
                value = string.Format(CultureInfo.CurrentUICulture, value, args);

            return new LocalizedString(name, value, !found);
        }
    }
}
