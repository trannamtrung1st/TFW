using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TFW.Cross.Entities;
using TFW.Cross.Models;

namespace TFW.Business.Logics
{
    public interface IAppUserLogic
    {
        Task<GetListResponseModel<AppUserResponseModel>> GetListAsync(
            GetAppUserListRequestModel requestModel);

        IQueryable<AppUser> QueryById(string id);
        
        IQueryable<AppUser> QueryByUsername(string username);

        PrincipalInfo MapToPrincipalInfo(ClaimsPrincipal principal);

        Task<ValidationData> ValidateGetListAsync(
            PrincipalInfo principal, GetAppUserListRequestModel requestModel);
    }
}
