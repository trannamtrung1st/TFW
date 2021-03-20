using System.Linq;
using System.Text;
using TFW.Framework.Configuration;

namespace TFW.Cross.Models.Setting
{
    public static class Settings
    {
        public static AppSettings App { get; set; }
        public static JwtSettings Jwt { get; set; }
        public static SerilogSettings Serilog { get; set; }
        public static ISecretsManager SecretsManager { get; set; }
    }
}
