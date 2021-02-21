using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TFW.Cross.Models.AppUser;
using TFW.Cross.Models.Common;

namespace TFW.Business.Logics
{
    public interface IAppUserLogic
    {
        Task<GetListResponseModel<AppUserResponseModel>> GetListAsync(
            GetAppUserListRequestModel requestModel, Type projectionType = null);

        Task<GetListResponseModel<AppUserResponseModel>> GetListDeletedAppUserAsync();

        PrincipalInfo MapToPrincipalInfo(ClaimsPrincipal principal);
    }
}
