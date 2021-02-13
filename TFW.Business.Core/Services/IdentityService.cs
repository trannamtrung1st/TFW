using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFW.Business.Logics;
using TFW.Business.Services;
using TFW.Cross.Entities;
using TFW.Cross.Models;
using TFW.Data;
using TFW.Framework.AutoMapper;

namespace TFW.Business.Core.Services
{
    public class IdentityService : BaseService
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
            DynamicQueryAppUserModel queryModel)
        {
            var response = await _appUserLogic.GetListAppUserAsync(queryModel);
            return response;
        }
        #endregion
    }

}
