using Microsoft.AspNetCore.Http.Extensions;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using TFW.Docs.Cross;
using TFW.Docs.Cross.Models.Common;

namespace TFW.Docs.ApiClient
{
    public interface ISettingClient
    {
        Task<(AppResult<bool> Result, HttpResponseMessage Response)> GetInitStatusAsync();
    }

    public class SettingClient : BaseClient, ISettingClient
    {
        public SettingClient(HttpClient client) : base(client)
        {
        }

        public async Task<(AppResult<bool> Result, HttpResponseMessage Response)> GetInitStatusAsync()
        {
            var resp = await http.GetAsync(
                string.Join('/', Routing.Controller.Setting.Route, Routing.Controller.Setting.InitStatus));
            return (await HandleJsonAsync<AppResult<bool>>(resp), resp);
        }
    }
}
