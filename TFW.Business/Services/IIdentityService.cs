using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TFW.Cross.Models.AppUser;
using TFW.Cross.Models.Common;
using TFW.Cross.Models.Identity;

namespace TFW.Business.Services
{
    public interface IIdentityService
    {
        Task<GetListResponseModel<TModel>> GetListAppUsersAsync<TModel>(
            GetListAppUsersRequestModel requestModel, ParsingConfig parsingConfig = null);

        Task<GetListResponseModel<TModel>> GetListDeletedAppUsersAsync<TModel>();

        Task<UserProfileModel> GetUserProfileAsync(string userId);

        Task RegisterAsync(RegisterModel model);

        Task<GetListResponseModel<TModel>> GetListRolesAsync<TModel>();

        Task<TokenResponseModel> ProvideTokenAsync(RequestTokenModel requestModel);

        PrincipalInfo MapToPrincipalInfo(ClaimsPrincipal principal);
    }
}
