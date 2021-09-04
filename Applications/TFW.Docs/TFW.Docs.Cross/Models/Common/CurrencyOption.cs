using Newtonsoft.Json;

namespace TFW.Docs.Cross.Models.Common
{
    public class CurrencyOption
    {
        [JsonProperty("isoCurrencySymbol")]
        public string ISOCurrencySymbol { get; set; }

        [JsonProperty("currencySymbol")]
        public string CurrencySymbol { get; set; }
    }
}
