﻿using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace TFW.Framework.Http.Extensions
{
    public static class HttpClientExtensions
    {
        public static Task<HttpResponseMessage> PatchAsJsonAsync<T>(this HttpClient client, string uri, T obj)
        {
            return client.PatchAsync(uri, JsonContent.Create(obj));
        }

        public static Task<HttpResponseMessage> PostAsFormAsync(this HttpClient client, string uri, IEnumerable<KeyValuePair<string, string>> form)
        {
            return client.PostAsync(uri, new FormUrlEncodedContent(form));
        }

        public static Task<HttpResponseMessage> PutAsFormAsync(this HttpClient client, string uri, IEnumerable<KeyValuePair<string, string>> form)
        {
            return client.PutAsync(uri, new FormUrlEncodedContent(form));
        }

        public static Task<HttpResponseMessage> PatchAsFormAsync(this HttpClient client, string uri, IEnumerable<KeyValuePair<string, string>> form)
        {
            return client.PatchAsync(uri, new FormUrlEncodedContent(form));
        }
    }
}
