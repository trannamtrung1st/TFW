using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TFW.Cross.Models.AppUser;
using TFW.Cross.Models.Common;

namespace TFW.Business.Services
{
    public interface IIdentityService
    {
        Task<GetListResponseModel<AppUserResponseModel>> GetListAppUserAsync(
            GetAppUserListRequestModel requestModel);

        PrincipalInfo MapToPrincipalInfo(ClaimsPrincipal principal);
    }
}
