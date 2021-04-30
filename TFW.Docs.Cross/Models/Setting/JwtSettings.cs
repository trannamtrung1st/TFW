using System.Collections.Generic;

namespace TFW.Docs.Cross.Models.Setting
{
    public class JwtSettings
    {
        public const string ConfigKey = nameof(JwtSettings) + ":" + nameof(SecretKey);

        public string Issuer { get; set; }
        public IEnumerable<string> Audiences { get; set; }
        public string SecretKey { get; set; }
        public int TokenExpiresInSeconds { get; set; }
        public int RefreshTokenExpiresInSeconds { get; set; }
    }
}
