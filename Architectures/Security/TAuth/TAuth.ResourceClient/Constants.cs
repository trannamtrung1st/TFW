namespace TAuth.ResourceClient
{
    public class AppSettings
    {
        public string ResourceApiUrl { get; set; }
        public string IdpUrl { get; set; }
    }

    public static class HttpClientConstants
    {
        public const string ResourceAPI = nameof(ResourceAPI);
        public const string IdentityAPI = nameof(IdentityAPI);
    }
}
