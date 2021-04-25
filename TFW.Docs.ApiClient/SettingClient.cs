using Microsoft.AspNetCore.Http.Extensions;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TFW.Docs.Cross;
using TFW.Docs.Cross.Models.Common;
using TFW.Docs.Cross.Models.Identity;

namespace TFW.Docs.ApiClient
{
    public interface ISettingClient
    {
        Task<(AppResult<bool> Result, HttpResponseMessage Response)> GetInitStatusAsync();
    }

    public class SettingClient : BaseClient, ISettingClient
    {
        public SettingClient(HttpClient client, ClientInfo clientInfo) : base(client, clientInfo)
        {
        }

        public async Task<(AppResult<bool> Result, HttpResponseMessage Response)> GetInitStatusAsync()
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get,
                string.Join('/', Routing.Controller.Setting.Route, Routing.Controller.Setting.InitStatus));

            requestMessage.Headers.Authorization = new AuthenticationHeaderValue(
                SecurityConsts.ClientAuthenticationScheme,
                Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientInfo.ClientId}:{clientInfo.ClientSecret}")));

            var resp = await http.SendAsync(requestMessage);
            return (await HandleJsonAsync<AppResult<bool>>(resp), resp);
        }
    }
}
