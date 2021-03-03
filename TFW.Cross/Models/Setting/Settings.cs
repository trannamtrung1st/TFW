using System.Linq;
using System.Text;

namespace TFW.Cross.Models.Setting
{
    public static class Settings
    {
        public static AppSettings App { get; set; }
        public static JwtSettings Jwt { get; set; }
    }
}
