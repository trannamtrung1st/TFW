using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TFW.Framework.Http.Contents;

namespace TFW.Framework.Http.Helpers
{
    public static class HttpClientExtensions
    {
        public static Task<HttpResponseMessage> PostAsJsonAsync(this HttpClient client, string uri, object obj)
        {
            return client.PostAsync(uri, new JsonContent(obj));
        }

        public static Task<HttpResponseMessage> PutAsJsonAsync(this HttpClient client, string uri, object obj)
        {
            return client.PutAsync(uri, new JsonContent(obj));
        }

        public static Task<HttpResponseMessage> PatchAsJsonAsync(this HttpClient client, string uri, object obj)
        {
            return client.PatchAsync(uri, new JsonContent(obj));
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

        public static async Task<T> ReadFromJsonAsync<T>(this HttpContent message)
        {
            return JsonConvert.DeserializeObject<T>(await message.ReadAsStringAsync());
        }
    }
}
