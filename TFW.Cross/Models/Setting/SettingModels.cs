using System;
using System.Collections.Generic;
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
    }

    public class JwtSettings
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string SecretKey { get; set; }
    }
}
