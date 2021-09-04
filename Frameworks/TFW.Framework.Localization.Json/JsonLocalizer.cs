using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using TFW.Framework.Common.Extensions;

namespace TFW.Framework.Localization.Json
{
    public class JsonLocalizer<T> : IStringLocalizer, IStringLocalizer<T>
    {
        private readonly JsonLocalizerOptions _options;
        private readonly Type _contextType;

        public JsonLocalizer(IOptions<JsonLocalizerOptions> options)
        {
            _contextType = typeof(T);
            _options = options.Value;
        }

        public LocalizedString this[string name] => GetString(name);

        public LocalizedString this[string name, params object[] arguments] => GetString(name, arguments);

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            var currentUiCulture = CultureInfo.CurrentUICulture.Name;
            var currentUiLang = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            var typedResources = GetResources();
            IEnumerable<LocalizedString> allStrings = new List<LocalizedString>();

            if (typedResources.Count > 0)
            {
                if (typedResources.ContainsKey(currentUiCulture))
                    allStrings = allStrings.Concat(
                        typedResources[currentUiCulture]
                            .Select(kvp => new LocalizedString(kvp.Key, kvp.Value)).ToArray());

                if (includeParentCultures && currentUiCulture != currentUiLang
                    && typedResources.ContainsKey(currentUiLang))
                    allStrings = allStrings.Concat(
                        typedResources[currentUiLang]
                            .Select(kvp => new LocalizedString(kvp.Key, kvp.Value)).ToArray());
            }

            return allStrings;
        }

        public IStringLocalizer WithCulture(CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        private LocalizedString GetString(string name, params object[] args)
        {
            var currentUiCulture = CultureInfo.CurrentUICulture.Name;
            var currentUiLang = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            string value = null; bool found = false;
            var typedResources = GetResources();
            IDictionary<string, string> resources;

            if (typedResources.Count > 0)
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

        private IDictionary<string, IDictionary<string, string>> GetResources()
        {
            return _options.Cache.GetOrCreate(_contextType, (entry) =>
            {
                var typedResources = new Dictionary<string, IDictionary<string, string>>();
                var basePath = $"{_contextType.Namespace}".Replace('.', Path.DirectorySeparatorChar);
                var contextName = _contextType.GetNameWithoutGenericParameters();

                if (!string.IsNullOrEmpty(_options.ResourcesPath))
                    basePath = Path.Combine(_options.BasePath, _options.ResourcesPath, basePath);

                var files = Directory.GetFiles(basePath, $"{contextName}*.json", SearchOption.TopDirectoryOnly);
                long size = 0;

                foreach (var file in files)
                {
                    var fileName = Path.GetFileName(file);
                    if (!fileName.StartsWith($"{contextName}.")) continue;

                    var json = File.ReadAllText(file);
                    size += json.Length;
                    string key = string.Empty;

                    if (file != $"{contextName}.json")
                    {
                        var beginCultureIdx = contextName.Length + 1;
                        key = fileName.Substring(beginCultureIdx,
                            fileName.LastIndexOf('.') - beginCultureIdx);
                    }

                    typedResources[key] = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
                }

                _options.CacheEntryConfiguration?.Invoke(entry);
                entry.SetSize(size);
                return typedResources;
            });
        }

    }
}
