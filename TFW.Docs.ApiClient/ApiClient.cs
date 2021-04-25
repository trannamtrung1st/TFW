using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TFW.Docs.ApiClient
{
    public interface IApiClient : IDisposable
    {
        HttpClient Http { get; }
        IUserClient User { get; }
    }

    public class ApiClient : IApiClient
    {
        public ApiClient(string baseUrl)
        {
            Http = new HttpClient()
            {
                BaseAddress = new Uri(baseUrl)
            };
            User = new UserClient(Http);
        }

        public HttpClient Http { get; }
        public IUserClient User { get; }

        public void Dispose()
        {
            Http.Dispose();
        }
    }
}
