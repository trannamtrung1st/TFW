using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TFW.Docs.Cross.Models.AppUser;
using TFW.Docs.Cross.Models.Common;
using TFW.Docs.Cross.Models.Identity;

namespace TFW.Docs.Business.Services
{
    public interface IIdentityService
    {
        Task<ClientInfo> AuthenticateClientAsync(string clientId, string clientSecret);

        ClaimsPrincipal MapToClaimsPrincipal(ClientInfo clientInfo);

        Task<ListResponseModel<TModel>> GetListAppUserAsync<TModel>(
            ListAppUserRequestModel requestModel, ParsingConfig parsingConfig = null);

        Task<int> GetTotalUserCountAsync();

        Task<ListResponseModel<TModel>> GetListDeletedAppUserAsync<TModel>();

        Task<UserProfileModel> GetUserProfileAsync(int userId);

        Task InitializeAsync(RegisterModel model);

        Task RegisterAsync(RegisterModel model);

        Task AddUserRolesAsync(ChangeUserRolesBaseModel model);

        Task RemoveUserRolesAsync(ChangeUserRolesBaseModel model);

        Task<ListResponseModel<TModel>> GetListRoleAsync<TModel>();

        Task<TokenResponseModel> ProvideTokenAsync(RequestTokenModel requestModel);

        PrincipalInfo MapToPrincipalInfo(ClaimsPrincipal principal);
    }
}
