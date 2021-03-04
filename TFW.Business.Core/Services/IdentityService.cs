using System;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TFW.Business.Logics;
using TFW.Business.Services;
using TFW.Cross.Models.AppRole;
using TFW.Cross.Models.AppUser;
using TFW.Cross.Models.Common;
using TFW.Framework.DI.Attributes;

namespace TFW.Business.Core.Services
{
    [ScopedService(ServiceType = typeof(IIdentityService))]
    public class IdentityService : BaseService, IIdentityService
    {
        private readonly IAppUserLogic _appUserLogic;
        private readonly IAppRoleLogic _appRoleLogic;

        public IdentityService(IAppUserLogic appUserLogic,
            IAppRoleLogic appRoleLogic)
        {
            _appUserLogic = appUserLogic;
            _appRoleLogic = appRoleLogic;
        }

        #region AppUser
        public Task<GetListResponseModel<GetListAppUsersResponseModel>> GetListAppUsersAsync(
            GetListAppUsersRequestModel requestModel)
        {
            return _appUserLogic.GetListAsync(requestModel);
        }

        public Task<GetListResponseModel<GetListAppUsersResponseModel>> GetListDeletedAppUsersAsync()
        {
            return _appUserLogic.GetListDeletedAsync();
        }
        #endregion

        #region AppRole
        public Task<GetListResponseModel<GetListRolesResponseModel>> GetListRolesAsync()
        {
            return _appRoleLogic.GetListAsync();
        }
        #endregion

        #region Common
        public PrincipalInfo MapToPrincipalInfo(ClaimsPrincipal principal)
        {
            var principalInfo = _appUserLogic.MapToPrincipalInfo(principal);

            return principalInfo;
        }
        #endregion
    }

}
