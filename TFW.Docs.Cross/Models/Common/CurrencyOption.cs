using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

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
