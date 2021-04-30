﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Docs.Cross.Models.Identity
{
    public class TokenResponseModel
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("user_id")]
        public int user_id { get; set; }

        [JsonExtensionData]
        public IDictionary<string, JToken> Extra { get; set; } = new Dictionary<string, JToken>();
    }
}
