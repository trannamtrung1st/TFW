using Microsoft.Extensions.Caching.Memory;
using System;

namespace TFW.Framework.Localization.Json
{
    public class JsonLocalizerOptions
    {
        public JsonLocalizerOptions()
        {
        }

        public string ResourcesPath { get; set; }
        public string BasePath { get; set; } = string.Empty;
        public Action<ICacheEntry> CacheEntryConfiguration { get; set; }
        public IMemoryCache Cache { get; set; }
    }
}
