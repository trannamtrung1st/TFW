﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TFW.Docs.Business.Core.Queries.AppUser;
using TFW.Docs.Business.Services;
using TFW.Docs.Cross;
using TFW.Docs.Cross.Entities;
using TFW.Docs.Cross.Exceptions;
using TFW.Docs.Cross.Models.AppUser;
using TFW.Docs.Cross.Models.Common;
using TFW.Docs.Cross.Models.Identity;
using TFW.Docs.Cross.Models.Setting;
using TFW.Docs.Cross.Providers;
using TFW.Docs.Data;
using TFW.Framework.AutoMapper;
using TFW.Framework.Common.Extensions;
using TFW.Framework.DI.Attributes;
using TFW.Framework.EFCore.Query;
using TFW.Framework.Security.Extensions;

namespace TFW.Docs.Business.Core.Services
{
    [ScopedService(ServiceType = typeof(IIdentityService))]
    public class IdentityService : BaseService, IIdentityService
    {
        private readonly UserManager<AppUserEntity> _userManager;
        private readonly SignInManager<AppUserEntity> _signInManager;

        public IdentityService(DataContext dbContext,
            IStringLocalizer<ResultCode> resultLocalizer,
            IBusinessContextProvider contextProvider,
            UserManager<AppUserEntity> userManager,
            SignInManager<AppUserEntity> signInManager) : base(dbContext, resultLocalizer, contextProvider)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        #region AppUser
        public async Task<ListResponseModel<TModel>> GetListAppUserAsync<TModel>(
            ListAppUserRequestModel requestModel, ParsingConfig parsingConfig = null)
        {
            #region Validation
            var userInfo = contextProvider.BusinessContext.PrincipalInfo;
            var validationData = new ValidationData(resultLocalizer);

            // validation logic here

            if (!validationData.IsValid)
                throw validationData.BuildException();
            #endregion

            IQueryable<AppUserEntity> query = dbContext.Users.AsNoTracking();

            #region Filter
            if (requestModel.Ids?.Any() == true)
                query = query.ByIds(requestModel.Ids);

            if (requestModel.UserName != null)
                query = query.ByUsername(requestModel.UserName);

            if (!string.IsNullOrWhiteSpace(requestModel.SearchTerm))
                query = query.BySearchTerm(requestModel.SearchTerm);

            if (requestModel.RegisteredFromDate != null)
                query = query.CreatedFrom(requestModel.RegisteredFromDate.Value);

            if (requestModel.RegisteredToDate != null)
                query = query.CreatedTo(requestModel.RegisteredToDate.Value);
            #endregion

            var orgQuery = query;

            #region Sorting
            var sortByArr = requestModel.GetSortByArr();
            if (!sortByArr.IsNullOrEmpty())
            {
                foreach (var field in sortByArr)
                {
                    var asc = field[0] == QueryConsts.SortAscPrefix;
                    var fieldName = field.Remove(0, 1);

                    switch (fieldName)
                    {
                        case ListAppUserRequestModel.SortByUsername:
                            {
                                if (asc)
                                    query = query.SequentialOrderBy(o => o.UserName);
                                else
                                    query = query.SequentialOrderByDesc(o => o.UserName);
                            }
                            break;
                        default:
                            throw AppValidationException.From(resultLocalizer, ResultCode.InvalidPagingRequest);
                    }
                }
            }
            #endregion

            if (requestModel.Page > 0)
                query = query.Limit(requestModel.Page, requestModel.PageLimit);

            #region Projection
            var projectionArr = requestModel.GetFieldsArr().Select(o => ListAppUserRequestModel.Projections[o]).ToArray();
            var projectionStr = string.Join(',', projectionArr);

            var projectedQuery = query.Select<TModel>(
                parsingConfig ?? DynamicLinqConsts.DefaultParsingConfig,
                $"new {typeof(TModel).FullName}({projectionStr})");
            #endregion

            var responseModels = await projectedQuery.ToArrayAsync();
            var response = new ListResponseModel<TModel>
            {
                List = responseModels,
            };

            if (requestModel.CountTotal)
                response.TotalCount = await orgQuery.CountAsync();

            return response;
        }

        public async Task<int> GetTotalUserCountAsync()
        {
            var total = await dbContext.Users.CountAsync();
            return total;
        }

        public async Task<ListResponseModel<TModel>> GetListDeletedAppUserAsync<TModel>()
        {
            var responseModels = await dbContext.QueryDeleted<AppUserEntity>()
                   .AsNoTracking().DefaultProjectTo<TModel>().ToArrayAsync();

            var response = new ListResponseModel<TModel>
            {
                List = responseModels,
                TotalCount = responseModels.Length
            };

            return response;
        }

        public async Task<UserProfileModel> GetUserProfileAsync(int userId)
        {
            #region Validation
            var userInfo = contextProvider.BusinessContext.PrincipalInfo;
            var validationData = new ValidationData(resultLocalizer);

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

        public async Task InitializeAsync(RegisterModel model)
        {
            #region Validation
            var userInfo = contextProvider.BusinessContext.PrincipalInfo;
            var validationData = new ValidationData(resultLocalizer);

            var hasAnyUser = await dbContext.Users.AnyAsync();
            if (hasAnyUser)
                validationData.Fail(code: ResultCode.Identity_AlreadyInitialized);

            if (!validationData.IsValid)
                throw validationData.BuildException();
            #endregion

            IdentityResult result;
            using (var trans = dbContext.Database.BeginTransaction())
            {
                var appUser = new AppUserEntity
                {
                    UserName = model.Username,
                    FullName = model.FullName,
                    Email = model.Email
                };

                result = await CreateUserWithRolesTransactionAsync(appUser, model.Password,
                    new[] { RoleName.Administrator });

                if (result.Succeeded)
                {
                    trans.Commit();
                    return;
                }
            }

            foreach (var err in result.Errors)
                validationData.Fail(code: ResultCode.Identity_FailToRegisterUser, data: err);

            throw validationData.BuildException();
        }

        public async Task RegisterAsync(RegisterModel model)
        {
            #region Validation
            var userInfo = contextProvider.BusinessContext.PrincipalInfo;
            var validationData = new ValidationData(resultLocalizer);

            // validation logic here

            if (!validationData.IsValid)
                throw validationData.BuildException();
            #endregion

            IdentityResult result;
            using (var trans = dbContext.Database.BeginTransaction())
            {
                var appUser = new AppUserEntity
                {
                    UserName = model.Username,
                    FullName = model.FullName,
                    Email = model.Email
                };

                result = await CreateUserWithRolesTransactionAsync(appUser, model.Password);

                if (result.Succeeded)
                {
                    trans.Commit();
                    return;
                }
            }

            foreach (var err in result.Errors)
                validationData.Fail(code: ResultCode.Identity_FailToRegisterUser, data: err);

            throw validationData.BuildException();
        }

        public async Task AddUserRolesAsync(ChangeUserRolesBaseModel model)
        {
            #region Validation
            var userInfo = contextProvider.BusinessContext.PrincipalInfo;
            var validationData = new ValidationData(resultLocalizer);

            // validation logic here

            if (!validationData.IsValid)
                throw validationData.BuildException();
            #endregion

            var appUser = await _userManager.FindByNameAsync(model.Username);

            if (appUser == null)
                throw AppValidationException.From(resultLocalizer, ResultCode.EntityNotFound);

            var result = await _userManager.AddToRolesAsync(appUser, model.Roles);

            if (result.Succeeded) return;

            foreach (var err in result.Errors)
                validationData.Fail(code: ResultCode.Identity_FailToChangeUserRoles, data: err);

            throw validationData.BuildException();
        }

        public async Task RemoveUserRolesAsync(ChangeUserRolesBaseModel model)
        {
            #region Validation
            var userInfo = contextProvider.BusinessContext.PrincipalInfo;
            var validationData = new ValidationData(resultLocalizer);

            // validation logic here

            if (!validationData.IsValid)
                throw validationData.BuildException();
            #endregion

            var appUser = await _userManager.FindByNameAsync(model.Username);

            if (appUser == null)
                throw AppValidationException.From(resultLocalizer, ResultCode.EntityNotFound);

            var result = await _userManager.RemoveFromRolesAsync(appUser, model.Roles);

            if (result.Succeeded) return;

            foreach (var err in result.Errors)
                validationData.Fail(code: ResultCode.Identity_FailToChangeUserRoles, data: err);

            throw validationData.BuildException();
        }

        private async Task<IdentityResult> CreateUserWithRolesTransactionAsync(AppUserEntity appUser, string password,
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

        private void PrepareCreate(AppUserEntity appUser)
        {
        }
        #endregion

        #region AppRole
        public async Task<ListResponseModel<TModel>> GetListRoleAsync<TModel>()
        {
            #region Validation
            var userInfo = contextProvider.BusinessContext.PrincipalInfo;
            var validationData = new ValidationData(resultLocalizer);

            // validation logic here

            if (!validationData.IsValid)
                throw validationData.BuildException();
            #endregion

            var query = dbContext.Roles.AsNoTracking().OrderBy(o => o.Name);

            var responseModels = await query.DefaultProjectTo<TModel>().ToArrayAsync();

            var response = new ListResponseModel<TModel>
            {
                List = responseModels,
                TotalCount = await query.CountAsync()
            };

            return response;
        }
        #endregion

        #region OAuth
        public Task<ClientInfo> AuthenticateClientAsync(string clientId, string clientSecret)
        {
            var clientInfo = AppClients.Known.FirstOrDefault(client => client.ClientId == clientId &&
                (client.Type == ClientType.Public || client.ClientSecret == clientSecret));

            return Task.FromResult(clientInfo);
        }

        public ClaimsPrincipal MapToClaimsPrincipal(ClientInfo clientInfo)
        {
            var identity = new ClaimsIdentity(SecurityConsts.ClientAuthenticationScheme);

            identity.AddClaims(new[]
            {
                new Claim(identity.NameClaimType, clientInfo.ClientId),
                new Claim(ClaimTypes.GivenName, clientInfo.Name),
                new Claim(SecurityConsts.ClaimTypes.ClientType, clientInfo.Type.ToStringF())
            });

            var principal = new ClaimsPrincipal(identity);
            return principal;
        }

        public async Task<TokenResponseModel> ProvideTokenAsync(RequestTokenModel requestModel)
        {
            AppUserEntity entity = null;

            switch (requestModel.GrantType)
            {
                case SecurityConsts.GrantTypes.Password:
                    {
                        entity = await AuthenticateAsync(requestModel.Username, requestModel.Password);
                    }
                    break;
                case SecurityConsts.GrantTypes.RefreshToken:
                    {
                        var validResult = ValidateRefreshToken(requestModel.RefreshToken);

                        entity = await _userManager.FindByIdAsync(validResult.IdentityName());
                    }
                    break;
                default:
                    {
                        throw OAuthException.UnsupportedGrantType(description: nameof(requestModel.GrantType));
                    }
            }

            if (entity == null) throw OAuthException.InvalidGrant();

            var identity =
                await GetIdentityAsync(entity, JwtBearerDefaults.AuthenticationScheme);

            #region Handle scope
            if (requestModel.Scope != null)
            {
                // demo only, real scenario: validate requested scopes first --> ... 
                var scopes = requestModel.Scope.Split(',')
                    .Select(scope => new Claim(SecurityConsts.ClaimTypes.AppScope, scope.Trim())).ToArray();

                identity.AddClaims(scopes);
            }
            #endregion

            var principal = new ClaimsPrincipal(identity);

            var tokenResponse = GenerateTokenResponse(principal);

            return tokenResponse;
        }

        private async Task<AppUserEntity> AuthenticateAsync(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null || !(await _userManager.CheckPasswordAsync(user, password)))
                return null;

            return user;
        }

        private ClaimsPrincipal ValidateRefreshToken(string tokenStr)
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

        private async Task<ClaimsIdentity> GetIdentityAsync(AppUserEntity entity, string scheme)
        {
            var identity = new ClaimsIdentity(scheme);

            identity.AddClaim(new Claim(ClaimTypes.Name, $"{entity.Id}"));

            var claims = await _userManager.GetClaimsAsync(entity);
            var roles = await _userManager.GetRolesAsync(entity);

            foreach (var r in roles)
                claims.Add(new Claim(ClaimTypes.Role, r));

            identity.AddClaims(claims);

            return identity;
        }

        //for IdentityCookie
        private async Task<ClaimsPrincipal> GetApplicationPrincipalAsync(AppUserEntity entity)
        {
            var principal = await _signInManager.CreateUserPrincipalAsync(entity);

            var identity = principal.Identity as ClaimsIdentity;

            identity.AddClaim(new Claim(ClaimTypes.Name, $"{entity.Id}"));

            var claims = new List<Claim>();
            var roles = await _userManager.GetRolesAsync(entity);

            foreach (var r in roles)
                claims.Add(new Claim(ClaimTypes.Role, r));

            identity.AddClaims(claims);

            return principal;
        }

        private TokenResponseModel GenerateTokenResponse(ClaimsPrincipal principal)
        {
            var jwtSettings = Settings.Get<JwtSettings>();

            #region Generate JWT Token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.Default.GetBytes(jwtSettings.SecretKey);
            var issuer = jwtSettings.Issuer;
            var identity = principal.Identity as ClaimsIdentity;
            var audClaims = jwtSettings.Audiences.Select(aud => new Claim(JwtRegisteredClaimNames.Aud, aud)).ToArray();

            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, principal.IdentityName()));
            identity.AddClaims(audClaims);

            var utcNow = DateTime.UtcNow;

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = issuer,
                //Audience = audience,
                Subject = identity,
                IssuedAt = utcNow,
                Expires = utcNow.AddSeconds(jwtSettings.TokenExpiresInSeconds),
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
            resp.ExpiresIn = jwtSettings.TokenExpiresInSeconds;
            resp.RefreshTokenExpiresIn = jwtSettings.RefreshTokenExpiresInSeconds;
            resp.Roles = identity.FindAll(identity.RoleClaimType).Select(c => c.Value).ToArray();
            resp.Permissions = identity.FindAll(SecurityConsts.ClaimTypes.Permissions).Select(c => c.Value).ToArray();

            int userId;
            if (int.TryParse(identity.Name, out userId))
                resp.UserId = userId;

            #region Refresh Token
            key = Encoding.Default.GetBytes(jwtSettings.SecretKey);
            issuer = jwtSettings.Issuer;

            identity = new ClaimsIdentity(
                identity.Claims.Where(c => c.Type == identity.NameClaimType),
                identity.AuthenticationType);
            identity.AddClaims(audClaims);

            tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = issuer,
                //Audience = audience,
                Subject = identity,
                IssuedAt = utcNow,
                Expires = utcNow.AddSeconds(jwtSettings.RefreshTokenExpiresInSeconds),
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
        #endregion

        #region Common
        public PrincipalInfo MapToPrincipalInfo(ClaimsPrincipal principal)
        {
            var principalInfo = principal.MapTo<PrincipalInfo>();

            return principalInfo;
        }
        #endregion
    }
}
