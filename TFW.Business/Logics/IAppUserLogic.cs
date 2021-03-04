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
        Task<GetListResponseModel<GetListAppUsersResponseModel>> GetListAsync(
            GetListAppUsersRequestModel requestModel, Type projectionType = null, ParsingConfig parsingConfig = null);

        Task<GetListResponseModel<GetListAppUsersResponseModel>> GetListDeletedAsync();

        PrincipalInfo MapToPrincipalInfo(ClaimsPrincipal principal);
    }
}
