using Microsoft.AspNetCore.Http.Extensions;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using TFW.Docs.Cross;
using TFW.Docs.Cross.Models.AppUser;
using TFW.Docs.Cross.Models.Common;

namespace TFW.Docs.ApiClient
{
    public interface IUserClient
    {
        Task<(AppResult<GetListResponseModel<GetListAppUsersResponseModel>> Result, HttpResponseMessage Response)> GetListAppUserAsync(
            GetListAppUsersRequestModel model = null);

        Task<(AppResult<int> Result, HttpResponseMessage Response)> GetTotalUserCountAsync();
    }

    public class UserClient : BaseClient, IUserClient
    {
        public UserClient(HttpClient client) : base(client)
        {
        }

        public async Task<(AppResult<GetListResponseModel<GetListAppUsersResponseModel>> Result, HttpResponseMessage Response)> GetListAppUserAsync(
            GetListAppUsersRequestModel model = null)
        {
            var queryBuilder = model?.BuildQuery();
            var uri = $"{string.Join('/', Routing.Controller.User.Route, Routing.Controller.User.GetListAppUser)}{queryBuilder}";
            var resp = await http.GetAsync(uri);
            return (await HandleJsonAsync<AppResult<GetListResponseModel<GetListAppUsersResponseModel>>>(resp), resp);
        }

        public async Task<(AppResult<int> Result, HttpResponseMessage Response)> GetTotalUserCountAsync()
        {
            var resp = await http.GetAsync(
                string.Join('/', Routing.Controller.User.Route, Routing.Controller.User.GetTotalUserCount));
            return (await HandleJsonAsync<AppResult<int>>(resp), resp);
        }
    }
}
