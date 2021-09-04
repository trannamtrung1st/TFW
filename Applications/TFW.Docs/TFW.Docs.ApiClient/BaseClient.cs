using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TFW.Docs.ApiClient.Exceptions;
using TFW.Docs.Cross.Models.Identity;

namespace TFW.Docs.ApiClient
{
    public abstract class BaseClient
    {
        protected readonly HttpClient http;
        protected readonly ClientInfo clientInfo;

        public BaseClient(HttpClient client, ClientInfo clientInfo)
        {
            this.http = client;
            this.clientInfo = clientInfo;
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
