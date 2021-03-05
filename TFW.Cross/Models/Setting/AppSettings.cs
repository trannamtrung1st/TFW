using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;

namespace TFW.Cross.Models.Setting
{
    public class AppSettings
    {
        public string Name { get; set; }
        public int TokenExpiresInSeconds { get; set; }
        public int RefreshTokenExpiresInSeconds { get; set; }

        private IEnumerable<string> _supportedCultureNames;
        public IEnumerable<string> SupportedCultureNames
        {
            get => _supportedCultureNames; set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                _supportedCultureNames = value;
                _supportedCultureInfos = _supportedCultureNames.Select(o => CultureInfo.GetCultureInfo(o)).ToImmutableArray();
            }
        }

        private IEnumerable<CultureInfo> _supportedCultureInfos = ImmutableArray.Create(CultureInfo.CurrentCulture);
        public IEnumerable<CultureInfo> SupportedCultureInfos => _supportedCultureInfos;

        private IEnumerable<string> _supportedRegionNames;
        public IEnumerable<string> SupportedRegionNames
        {
            get => _supportedRegionNames; set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                _supportedRegionNames = value;
                _supportedRegionInfos = _supportedRegionNames.Select(o => new RegionInfo(o)).ToImmutableArray();
            }
        }

        private IEnumerable<RegionInfo> _supportedRegionInfos = ImmutableArray.Create(RegionInfo.CurrentRegion);
        public IEnumerable<RegionInfo> SupportedRegionInfos => _supportedRegionInfos;
    }
}
