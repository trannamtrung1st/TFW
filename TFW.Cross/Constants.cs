using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TFW.Cross.Models.Setting;

namespace TFW.Cross
{
    public static class QueryFilterName
    {
        public const string AnotherFilter1 = nameof(AnotherFilter1);
        public const string AnotherFilter2 = nameof(AnotherFilter2);
    }

    public class RequestDataKey
    {
        public const string PrincipalInfo = nameof(PrincipalInfo);
    }

    public static class QueryConsts
    {
        public const char SortAscPrefix = 'a';
        public const int DefaultPage = 1;
        public const int DefaultPageLimit = 100;
    }

    public static class ApiEndpoint
    {
        public const string Auth = "auth";
        public const string UserApi = "api/users";
        public const string RoleApi = "api/roles";
        public const string ReferenceDataApi = "api/ref";
        public const string Error = "error";
    }

    public static class TimeZoneConsts
    {
        public static readonly TimeZoneInfo Default = TimeZoneInfo.Local;
    }

    public static class ResultCodeGroup
    {
        public const int Common = 1;

        public const int Identity = 1000;

        public const int Setting = 2000;
    }

    public enum ResultCode
    {
        #region Common
        [Display(Name = "Unknown error")]
        UnknownError = ResultCodeGroup.Common,

        [Display(Name = "Success")]
        Success = ResultCodeGroup.Common + 1,

        [Display(Name = "Fail")]
        Fail = ResultCodeGroup.Common + 2,

        [Display(Name = "Fail validation")]
        FailValidation = ResultCodeGroup.Common + 3,

        [Display(Name = "Not found")]
        NotFound = ResultCodeGroup.Common + 4,

        [Display(Name = "Unsupported")]
        Unsupported = ResultCodeGroup.Common + 5,

        [Display(Name = "Can not delete because of dependencies")]
        DependencyDeleteFail = ResultCodeGroup.Common + 6,

        [Display(Name = "Unauthorized")]
        Unauthorized = ResultCodeGroup.Common + 7,

        [Display(Name = "Access denied")]
        AccessDenied = ResultCodeGroup.Common + 8,

        [Display(Name = "Invalid paging request")]
        InvalidPagingRequest = ResultCodeGroup.Common + 9,

        [Display(Name = "Invalid sorting request")]
        InvalidSortingRequest = ResultCodeGroup.Common + 10,

        [Display(Name = "Invalid projection request")]
        InvalidProjectionRequest = ResultCodeGroup.Common + 11,

        [Display(Name = "Entity not found")]
        EntityNotFound = ResultCodeGroup.Common + 12,
        #endregion

        #region Identity
        [Display(Name = "Invalid register request")]
        Identity_InvalidRegisterRequest = ResultCodeGroup.Identity,

        [Display(Name = "Fail to register user")]
        Identity_FailToRegisterUser = ResultCodeGroup.Identity + 1,

        [Display(Name = "Fail to change user roles")]
        Identity_FailToChangeUserRoles = ResultCodeGroup.Identity + 2,

        [Display(Name = "Invalid change user roles request")]
        Identity_InvalidChangeUserRolesRequest = ResultCodeGroup.Identity + 3,
        #endregion

        #region Setting
        Setting_InvalidChangeSmtpOptionRequest = ResultCodeGroup.Setting,
        #endregion
    }

    public static class SecurityConsts
    {
        public const string OAuth2 = nameof(OAuth2);

        public static class GrantType
        {
            public const string Password = "password";
            public const string RefreshToken = "refresh_token";
        }

        public static class ClaimType
        {
            public const string AppScope = "appscope";
        }

        public static readonly TokenValidationParameters DefaultTokenParameters = new TokenValidationParameters()
        {
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = Settings.Jwt.Issuer,
            ValidAudience = Settings.Jwt.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.Default.GetBytes(Settings.Jwt.SecretKey)),
            ClockSkew = TimeSpan.Zero
        };
    }

    public static class RoleName
    {
        public static IEnumerable<string> All = ImmutableArray.Create(Administrator);

        public const string Administrator = nameof(Administrator);
    }

    public static class EntityTableName
    {
        public const string AppUser = "AspNetUsers";
        public const string AppRole = "AspNetRoles";
        public const string AppUserRole = "AspNetUserRoles";
        public const string Note = nameof(Entities.Note);
        public const string NoteCategory = nameof(Entities.NoteCategory);
    }

}
