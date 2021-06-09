using Microsoft.AspNetCore.Http.Extensions;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using TFW.Docs.Cross;
using TFW.Docs.Cross.Models.AppUser;
using TFW.Docs.Cross.Models.Common;
using TFW.Docs.Cross.Models.Identity;

namespace TFW.Docs.ApiClient
{
    public interface IUserClient
    {
        Task<(AppResult<ListResponseModel<ListAppUserModel>> Result, HttpResponseMessage Response)> GetListAppUserAsync(
            ListAppUserRequestModel model = null);

        Task<(AppResult<int> Result, HttpResponseMessage Response)> GetTotalUserCountAsync();
    }

    public class UserClient : BaseClient, IUserClient
    {
        public UserClient(HttpClient client, ClientInfo clientInfo) : base(client, clientInfo)
        {
        }

        public async Task<(AppResult<ListResponseModel<ListAppUserModel>> Result, HttpResponseMessage Response)> GetListAppUserAsync(
            ListAppUserRequestModel model = null)
        {
            var queryBuilder = model?.BuildQuery();
            var uri = $"{string.Join('/', Routing.Controller.User.Route, Routing.Controller.User.GetListAppUser)}{queryBuilder}";
            var resp = await http.GetAsync(uri);
            return (await HandleJsonAsync<AppResult<ListResponseModel<ListAppUserModel>>>(resp), resp);
        }

        public async Task<(AppResult<int> Result, HttpResponseMessage Response)> GetTotalUserCountAsync()
        {
            var resp = await http.GetAsync(
                string.Join('/', Routing.Controller.User.Route, Routing.Controller.User.GetTotalUserCount));
            return (await HandleJsonAsync<AppResult<int>>(resp), resp);
        }
    }
}
