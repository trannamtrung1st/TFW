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
        Task<GetListResponseModel<TModel>> GetListAppUsersAsync<TModel>(
            GetListAppUsersRequestModel requestModel, ParsingConfig parsingConfig = null);

        Task<GetListResponseModel<TModel>> GetListDeletedAppUsersAsync<TModel>();

        Task<UserProfileModel> GetUserProfileAsync(int userId);

        Task RegisterAsync(RegisterModel model);

        Task AddUserRolesAsync(ChangeUserRolesBaseModel model);

        Task RemoveUserRolesAsync(ChangeUserRolesBaseModel model);

        Task<GetListResponseModel<TModel>> GetListRolesAsync<TModel>();

        Task<TokenResponseModel> ProvideTokenAsync(RequestTokenModel requestModel);

        PrincipalInfo MapToPrincipalInfo(ClaimsPrincipal principal);
    }
}
