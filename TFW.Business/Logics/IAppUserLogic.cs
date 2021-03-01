using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
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
            GetAppUserListRequestModel requestModel, Type projectionType = null, ParsingConfig parsingConfig = null);

        Task<GetListResponseModel<AppUserResponseModel>> GetListDeletedAppUserAsync();

        PrincipalInfo MapToPrincipalInfo(ClaimsPrincipal principal);
    }
}
