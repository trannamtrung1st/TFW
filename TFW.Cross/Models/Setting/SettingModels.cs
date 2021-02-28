using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace TFW.Cross.Models.Setting
{
    public static class Settings
    {
        public static AppSettings App { get; set; }
        public static JwtSettings Jwt { get; set; }
    }

    public class AppSettings
    {
        public string Name { get; set; }

        private string[] _supportedCultureNames;
        public string[] SupportedCultureNames
        {
            get => _supportedCultureNames; set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                _supportedCultureNames = value;
                _supportedCultureInfos = _supportedCultureNames.Select(o => CultureInfo.GetCultureInfo(o)).ToArray();
            }
        }

        private CultureInfo[] _supportedCultureInfos = new[] { CultureInfo.CurrentCulture };
        public CultureInfo[] SupportedCultureInfos => _supportedCultureInfos;
    }

    public class JwtSettings
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string SecretKey { get; set; }
    }
}
