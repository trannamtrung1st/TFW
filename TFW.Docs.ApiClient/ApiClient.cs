using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TFW.Docs.Cross.Models.Identity;

namespace TFW.Docs.ApiClient
{
    public interface IApiClient : IDisposable
    {
        ClientInfo UsingClient { get; }
        HttpClient Http { get; }
        IUserClient User { get; }
        ISettingClient Setting { get; }
    }

    public class ApiClient : IApiClient
    {
        public ApiClient(string baseUrl, ClientInfo clientInfo)
        {
            Http = new HttpClient()
            {
                BaseAddress = new Uri(baseUrl)
            };
            UsingClient = clientInfo;
            User = new UserClient(Http, clientInfo);
            Setting = new SettingClient(Http, clientInfo);
        }

        public ClientInfo UsingClient { get; }
        public HttpClient Http { get; }
        public IUserClient User { get; }
        public ISettingClient Setting { get; }

        public void Dispose()
        {
            Http.Dispose();
        }
    }
}
