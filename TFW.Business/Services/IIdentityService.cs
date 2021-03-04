using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TFW.Cross.Models.AppRole;
using TFW.Cross.Models.AppUser;
using TFW.Cross.Models.Common;

namespace TFW.Business.Services
{
    public interface IIdentityService
    {
        Task<GetListResponseModel<GetListAppUsersResponseModel>> GetListAppUsersAsync(
            GetListAppUsersRequestModel requestModel);

        Task<GetListResponseModel<GetListAppUsersResponseModel>> GetListDeletedAppUsersAsync();

        Task<GetListResponseModel<GetListRolesResponseModel>> GetListRolesAsync();

        PrincipalInfo MapToPrincipalInfo(ClaimsPrincipal principal);
    }
}
