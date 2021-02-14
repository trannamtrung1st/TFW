using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TFW.Business.Logics;
using TFW.Business.Services;
using TFW.Cross.Entities;
using TFW.Cross.Models;
using TFW.Data;
using TFW.Framework.AutoMapper;
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

        public IdentityService(DataContext dataContext,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            RoleManager<AppRole> roleManager,
            IAppUserLogic appUserLogic) : base(dataContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _appUserLogic = appUserLogic;
        }

        #region AppUser
        public async Task<GetListResponseModel<AppUserResponseModel>> GetListAppUserAsync(
            GetAppUserListRequestModel requestModel)
        {
            var response = await _appUserLogic.GetListAppUserAsync(requestModel);
            return response;
        }

        public async Task<ValidationData> ValidateGetAppUserListAsync(
            ClaimsPrincipal principal, GetAppUserListRequestModel requestModel)
        {
            var validationData = await _appUserLogic.ValidateGetAppUserListAsync(principal, requestModel);
            return validationData;
        }
        #endregion
    }

}
