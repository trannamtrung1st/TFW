namespace TFW.Docs.Cross.Helpers
{
    public static class I18nHelper
    {
        public static string GetCulture(string lang, string region)
        {
            return string.IsNullOrEmpty(region) ? lang : lang + "-" + region;
        }
    }
}
