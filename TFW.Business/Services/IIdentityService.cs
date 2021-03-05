using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TFW.Cross.Models.AppRole;
using TFW.Cross.Models.AppUser;
using TFW.Cross.Models.Common;
using TFW.Cross.Models.Identity;

namespace TFW.Business.Services
{
    public interface IIdentityService
    {
        Task<GetListResponseModel<GetListAppUsersResponseModel>> GetListAppUsersAsync(
            GetListAppUsersRequestModel requestModel, Type projectionType = null, ParsingConfig parsingConfig = null);

        Task<GetListResponseModel<GetListAppUsersResponseModel>> GetListDeletedAppUsersAsync();

        Task<GetListResponseModel<GetListRolesResponseModel>> GetListRolesAsync();

        Task<TokenResponseModel> ProvideTokenAsync(RequestTokenModel requestModel);

        PrincipalInfo MapToPrincipalInfo(ClaimsPrincipal principal);
    }
}
