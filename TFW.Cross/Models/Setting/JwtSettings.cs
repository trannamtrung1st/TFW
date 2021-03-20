namespace TFW.Cross.Models.Setting
{
    public class JwtSettings
    {
        public const string ConfigKey = nameof(JwtSettings) + ":" + nameof(SecretKey);

        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string SecretKey { get; set; }
        public int TokenExpiresInSeconds { get; set; }
        public int RefreshTokenExpiresInSeconds { get; set; }
    }
}
