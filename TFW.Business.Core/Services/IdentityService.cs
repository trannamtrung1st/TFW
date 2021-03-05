using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TFW.Business.Core.Queries;
using TFW.Business.Services;
using TFW.Cross;
using TFW.Cross.Entities;
using TFW.Cross.Models.AppUser;
using TFW.Cross.Models.Common;
using TFW.Cross.Models.Exceptions;
using TFW.Cross.Models.Identity;
using TFW.Cross.Models.Setting;
using TFW.Cross.Providers;
using TFW.Data.Core;
using TFW.Framework.AutoMapper.Helpers;
using TFW.Framework.Common.Helpers;
using TFW.Framework.DI.Attributes;
using TFW.Framework.EFCore.Queries;
using TFW.Framework.Security.Helpers;

namespace TFW.Business.Core.Services
{
    [ScopedService(ServiceType = typeof(IIdentityService))]
    public class IdentityService : BaseService, IIdentityService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public IdentityService(DataContext dbContext,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager) : base(dbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        #region AppUser
        public async Task<GetListResponseModel<TModel>> GetListAppUsersAsync<TModel>(
            GetListAppUsersRequestModel requestModel, ParsingConfig parsingConfig = null)
        {
            #region Validation
            var userInfo = BusinessContext.Current?.PrincipalInfo;
            var validationData = new ValidationData();

            // validation logic here

            if (!validationData.IsValid)
                throw validationData.BuildException();
            #endregion

            var queryModel = requestModel.MapTo<DynamicQueryAppUserModel>();
            IQueryable<AppUser> query = dbContext.Users.AsNoTracking();

            #region Filter
            if (queryModel.Id != null)
                query = query.ById(queryModel.Id);

            if (queryModel.UserName != null)
                query = query.ByUsername(queryModel.UserName);

            if (queryModel.SearchTerm != null)
                query = query.BySearchTerm(queryModel.SearchTerm);
            #endregion

            var orgQuery = query;

            #region Sorting
            if (!queryModel.SortBy.IsNullOrEmpty())
            {
                foreach (var field in queryModel.SortBy)
                {
                    var asc = field[0] == QueryConsts.SortAscPrefix;
                    var fieldName = field.Remove(0, 1);

                    switch (fieldName)
                    {
                        case DynamicQueryAppUserModel.SortByUsername:
                            {
                                if (asc)
                                    query = query.SequentialOrderBy(o => o.UserName);
                                else
                                    query = query.SequentialOrderByDesc(o => o.UserName);
                            }
                            break;
                        default:
                            throw AppValidationException.From(ResultCode.InvalidPagingRequest);
                    }
                }
            }
            #endregion

            if (queryModel.Page > 0)
                query = query.Limit(queryModel.Page, queryModel.PageLimit);

            #region Projection
            var projectionArr = queryModel.Fields.Select(o => DynamicQueryAppUserModel.Projections[o]).ToArray();
            var projectionStr = string.Join(',', projectionArr);

            var projectedQuery = query.Select<TModel>(
                parsingConfig ?? DynamicLinqEntityTypeProvider.DefaultParsingConfig,
                $"new {typeof(TModel).FullName}({projectionStr})");
            #endregion

            var responseModels = await projectedQuery.ToArrayAsync();
            var response = new GetListResponseModel<TModel>
            {
                List = responseModels,
            };

            if (queryModel.CountTotal)
                response.TotalCount = await orgQuery.CountAsync();

            return response;
        }

        public async Task<GetListResponseModel<TModel>> GetListDeletedAppUsersAsync<TModel>()
        {
            var responseModels = await dbContext.QueryDeleted<AppUser>()
                   .AsNoTracking().DefaultProjectTo<TModel>().ToArrayAsync();

            var response = new GetListResponseModel<TModel>
            {
                List = responseModels,
                TotalCount = responseModels.Length
            };

            return response;
        }

        public async Task<UserProfileModel> GetUserProfileAsync(string userId)
        {
            #region Validation
            var userInfo = BusinessContext.Current?.PrincipalInfo;
            var validationData = new ValidationData();

            // validation logic here

            if (!validationData.IsValid)
                throw validationData.BuildException();
            #endregion

            var userProfile = await dbContext.Users.AsNoTracking()
                .ById(userId).DefaultProjectTo<UserProfileModel>().FirstOrDefaultAsync();

            if (userProfile == null)
                throw validationData.Fail(code: ResultCode.EntityNotFound).BuildException();

            return userProfile;
        }

        public async Task RegisterAsync(RegisterModel model)
        {
            #region Validation
            var userInfo = BusinessContext.Current?.PrincipalInfo;
            var validationData = new ValidationData();

            // validation logic here

            if (!validationData.IsValid)
                throw validationData.BuildException();
            #endregion

            IdentityResult result;
            using (var trans = dbContext.Database.BeginTransaction())
            {
                var appUser = new AppUser
                {
                    UserName = model.username,
                    FullName = model.fullName,
                    Email = model.email
                };

                result = await CreateUserWithRolesTransactionAsync(appUser, model.password);

                if (result.Succeeded)
                {
                    trans.Commit();
                    return;
                }
            }

            foreach (var err in result.Errors)
                validationData.Fail(err.Description, ResultCode.Identity_FailToRegisterUser, err);

            throw validationData.BuildException();
        }
        #endregion

        #region AppRole
        public async Task<GetListResponseModel<TModel>> GetListRolesAsync<TModel>()
        {
            #region Validation
            var userInfo = BusinessContext.Current?.PrincipalInfo;
            var validationData = new ValidationData();

            // validation logic here

            if (!validationData.IsValid)
                throw validationData.BuildException();
            #endregion

            var query = dbContext.Roles.AsNoTracking()
                .OrderBy(o => o.Name);

            var responseModels = await query.DefaultProjectTo<TModel>().ToArrayAsync();

            var response = new GetListResponseModel<TModel>
            {
                List = responseModels,
                TotalCount = await query.CountAsync()
            };

            return response;
        }
        #endregion

        #region OAuth
        public async Task<TokenResponseModel> ProvideTokenAsync(RequestTokenModel requestModel)
        {
            AppUser entity = null;

            switch (requestModel.grant_type)
            {
                case SecurityConsts.GrantType.Password:
                    {
                        entity = await AuthenticateAsync(requestModel.username, requestModel.password);
                    }
                    break;
                case SecurityConsts.GrantType.RefreshToken:
                    {
                        var validResult = ValidateRefreshToken(requestModel.refresh_token);

                        entity = await _userManager.FindByIdAsync(validResult.IdentityName());
                    }
                    break;
                default:
                    {
                        throw OAuthException.UnsupportedGrantType(description: nameof(requestModel.grant_type));
                    }
            }

            if (entity == null) throw OAuthException.InvalidGrant();

            var identity =
                await GetIdentityAsync(entity, JwtBearerDefaults.AuthenticationScheme);

            #region Handle scope
            if (requestModel.scope != null)
            {
                // demo only, real scenario: validate requested scopes first --> ... 
                var scopes = requestModel.scope.Split(',')
                    .Select(scope => new Claim(SecurityConsts.ClaimType.AppScope, scope.Trim())).ToArray();

                identity.AddClaims(scopes);
            }
            #endregion

            var principal = new ClaimsPrincipal(identity);

            var tokenResponse = GenerateTokenResponse(principal);

            return tokenResponse;
        }
        #endregion

        #region Common
        public PrincipalInfo MapToPrincipalInfo(ClaimsPrincipal principal)
        {
            var principalInfo = principal.MapTo<PrincipalInfo>();

            return principalInfo;
        }
        #endregion

        private async Task<AppUser> AuthenticateAsync(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null || !(await _userManager.CheckPasswordAsync(user, password)))
                return null;

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

                throw OAuthException.InvalidGrant();
            }
        }

        private async Task<ClaimsIdentity> GetIdentityAsync(AppUser entity, string scheme)
        {
            var identity = new ClaimsIdentity(scheme);

            identity.AddClaim(new Claim(ClaimTypes.Name, entity.Id));

            var claims = await _userManager.GetClaimsAsync(entity);
            var roles = await _userManager.GetRolesAsync(entity);

            foreach (var r in roles)
                claims.Add(new Claim(ClaimTypes.Role, r));

            identity.AddClaims(claims);

            return identity;
        }

        //for IdentityCookie
        private async Task<ClaimsPrincipal> GetApplicationPrincipalAsync(AppUser entity)
        {
            var principal = await _signInManager.CreateUserPrincipalAsync(entity);

            var identity = principal.Identity as ClaimsIdentity;

            identity.AddClaim(new Claim(ClaimTypes.Name, entity.Id));

            var claims = new List<Claim>();
            var roles = await _userManager.GetRolesAsync(entity);

            foreach (var r in roles)
                claims.Add(new Claim(ClaimTypes.Role, r));

            identity.AddClaims(claims);

            return principal;
        }

        private TokenResponseModel GenerateTokenResponse(ClaimsPrincipal principal)
        {
            #region Generate JWT Token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.Default.GetBytes(Settings.Jwt.SecretKey);
            var issuer = Settings.Jwt.Issuer;
            var audience = Settings.Jwt.Audience;
            var identity = principal.Identity as ClaimsIdentity;

            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, principal.IdentityName()));

            var utcNow = DateTime.UtcNow;

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = issuer,
                Audience = audience,
                Subject = identity,
                IssuedAt = utcNow,
                Expires = utcNow.AddSeconds(Settings.App.TokenExpiresInSeconds),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                NotBefore = utcNow
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var tokenString = tokenHandler.WriteToken(token);
            #endregion

            var resp = new TokenResponseModel();
            resp.AccessToken = tokenString;
            resp.TokenType = JwtBearerDefaults.AuthenticationScheme;
            resp.ExpiresIn = Settings.App.TokenExpiresInSeconds;

            #region Refresh Token
            key = Encoding.Default.GetBytes(Settings.Jwt.SecretKey);
            issuer = Settings.Jwt.Issuer;
            audience = Settings.Jwt.Audience;

            identity = new ClaimsIdentity(
                identity.Claims.Where(c => c.Type == identity.NameClaimType),
                identity.AuthenticationType);

            tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = issuer,
                Audience = audience,
                Subject = identity,
                IssuedAt = utcNow,
                Expires = utcNow.AddSeconds(Settings.App.RefreshTokenExpiresInSeconds),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature),
                NotBefore = utcNow
            };

            token = tokenHandler.CreateToken(tokenDescriptor);
            tokenString = tokenHandler.WriteToken(token);

            resp.RefreshToken = tokenString;
            #endregion

            return resp;
        }

        private async Task<IdentityResult> CreateUserWithRolesTransactionAsync(AppUser appUser, string password,
            IEnumerable<string> roles = null)
        {
            PrepareCreate(appUser);

            var result = await _userManager.CreateAsync(appUser, password);

            if (!result.Succeeded)
                return result;

            if (!roles.IsNullOrEmpty())
                result = await _userManager.AddToRolesAsync(appUser, roles);

            return result;
        }

        #region Preparation
        private void PrepareCreate(AppUser appUser)
        {
            appUser.Id = Guid.NewGuid().ToString();
        }
        #endregion
    }
}
