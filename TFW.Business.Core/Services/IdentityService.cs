using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TFW.Business.Logics;
using TFW.Business.Services;
using TFW.Cross;
using TFW.Cross.Entities;
using TFW.Cross.Models.AppRole;
using TFW.Cross.Models.AppUser;
using TFW.Cross.Models.Common;
using TFW.Cross.Models.Exceptions;
using TFW.Cross.Models.Identity;
using TFW.Cross.Models.Setting;
using TFW.Framework.DI.Attributes;

namespace TFW.Business.Core.Services
{
    [ScopedService(ServiceType = typeof(IIdentityService))]
    public class IdentityService : BaseService, IIdentityService
    {
        private readonly IAppUserLogic _appUserLogic;
        private readonly IAppRoleLogic _appRoleLogic;
        private readonly UserManager<AppUser> _userManager;

        public IdentityService(
            UserManager<AppUser> userManager,
            IAppUserLogic appUserLogic,
            IAppRoleLogic appRoleLogic)
        {
            _userManager = userManager;
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

        #region OAuth
        public async Task<TokenResponseModel> ProvideTokenAsync(RequestTokenModel requestModel)
        {
            AppUser entity = null;

            switch (requestModel.grant_type)
            {
                case SecurityConsts.GrantType.Password:
                case null:
                    entity = await AuthenticateAsync(requestModel.username, requestModel.password);
                    break;
                case SecurityConsts.GrantType.RefreshToken:
                    {
                        var validResult = ValidateRefreshToken(requestModel.refresh_token);
                        entity = await _service.GetUserByIdAsync(validResult.Identity.Name);
                        if (entity == null)
                        {
                            return Unauthorized(new AppResultBuilder()
                                .Unauthorized(mess: "Invalid user identity"));
                        }
                    }
                    break;
                default:
                    {
                        var validationData = new ValidationData()
                            .Fail(AppResult.Unsupported(mess: "Unsupported grant type"));

                        throw AppValidationException.From(validationData);
                    }
            }

            var identity =
                await _service.GetIdentityAsync(entity, JwtBearerDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            var utcNow = DateTime.UtcNow;
            var props = new AuthenticationProperties()
            {
                IssuedUtc = utcNow,
                ExpiresUtc = utcNow.AddHours(WebApi.Settings.Instance.TokenValidHours)
            };
            props.Parameters["refresh_expires"] = utcNow.AddHours(
                WebApi.Settings.Instance.RefreshTokenValidHours);
            var resp = _service.GenerateTokenResponse(principal, props, model.scope);
            _logger.CustomProperties(entity).Info("Login user");
        }
        #endregion

        #region Common
        public PrincipalInfo MapToPrincipalInfo(ClaimsPrincipal principal)
        {
            var principalInfo = _appUserLogic.MapToPrincipalInfo(principal);

            return principalInfo;
        }
        #endregion

        private async Task<AppUser> AuthenticateAsync(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null || !(await _userManager.CheckPasswordAsync(user, password)))
                throw OAuthException.From(OAuthException.InvalidGrant);

            return user;
        }

        public ClaimsPrincipal ValidateRefreshToken(string tokenStr)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                SecurityToken secToken;

                return tokenHandler.ValidateToken(tokenStr, SecurityConsts.DefaultTokenParameters, out secToken);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);

                throw OAuthException.From(OAuthException.InvalidGrant);
            }
        }
    }

}
