using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using TFW.Docs.ApiClient.Exceptions;

namespace TFW.Docs.ApiClient
{
    public abstract class BaseClient
    {
        protected readonly HttpClient http;

        public BaseClient(HttpClient client)
        {
            this.http = client;
        }

        public virtual Task<T> HandleJsonAsync<T>(HttpResponseMessage resp)
        {
            switch (resp.StatusCode)
            {
                case HttpStatusCode.OK:
                    {
                        return resp.Content.ReadFromJsonAsync<T>();
                    }
                default:
                    throw new ApiException(resp);
            }
        }
    }
}
