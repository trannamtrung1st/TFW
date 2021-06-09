using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace TFW.Docs.Cross.Models.Setting
{
    public class AppSettings
    {
        public string Name { get; set; }

        private SwaggerSettings _swagger = new SwaggerSettings();
        public SwaggerSettings Swagger
        {
            get => _swagger; set
            {
                if (value is null) return;
                _swagger = value;
            }
        }

        private IEnumerable<string> _supportedCultureNames;
        public IEnumerable<string> SupportedCultureNames
        {
            get => _supportedCultureNames; set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                _supportedCultureNames = value;
                _supportedCultureInfos = _supportedCultureNames.Select(o => CultureInfo.GetCultureInfo(o)).ToArray();
            }
        }

        private IEnumerable<CultureInfo> _supportedCultureInfos = new[] { CultureInfo.CurrentCulture };
        public IEnumerable<CultureInfo> SupportedCultureInfos => _supportedCultureInfos;

        private IEnumerable<string> _supportedRegionNames;
        public IEnumerable<string> SupportedRegionNames
        {
            get => _supportedRegionNames; set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                _supportedRegionNames = value;
                _supportedRegionInfos = _supportedRegionNames.Select(o => new RegionInfo(o)).ToArray();
            }
        }

        private IEnumerable<RegionInfo> _supportedRegionInfos = new[] { RegionInfo.CurrentRegion };
        public IEnumerable<RegionInfo> SupportedRegionInfos => _supportedRegionInfos;
    }

    public class SwaggerSettings
    {
        public bool AddSwaggerAcceptLanguageHeader { get; set; }
        public bool AddSwaggerTimeZoneHeader { get; set; }
    }
}
