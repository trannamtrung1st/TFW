using Microsoft.AspNetCore.Identity;
using System;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TFW.Business.Logics;
using TFW.Business.Services;
using TFW.Cross.Entities;
using TFW.Cross.Models.AppUser;
using TFW.Cross.Models.Common;
using TFW.Framework.DI;

namespace TFW.Business.Core.Services
{
    [ScopedService(ServiceType = typeof(IIdentityService))]
    public class IdentityService : BaseService, IIdentityService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IAppUserLogic _appUserLogic;

        public IdentityService(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            RoleManager<AppRole> roleManager,
            IAppUserLogic appUserLogic)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _appUserLogic = appUserLogic;
        }

        #region AppUser
        public Task<GetListResponseModel<AppUserResponseModel>> GetListAppUserAsync(
            GetAppUserListRequestModel requestModel)
        {
            return _appUserLogic.GetListAsync(requestModel);
        }

        public Task<GetListResponseModel<AppUserResponseModel>> GetListDeletedAppUserAsync()
        {
            return _appUserLogic.GetListDeletedAppUserAsync();
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
