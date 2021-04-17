using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TFW.Cross.Models.Setting;
using TFW.Framework.Logging.Serilog.Web;

namespace TFW.Cross
{
    public static class ConfigConsts
    {
        public static class i18n
        {
            public const string ResourcePath = "Resources";
        }

        public static class CommandLine
        {
            public const string WindowsCmd = "cmd.exe";
        }

        public static class Mail
        {
            public const string PasswordKey = "Mail:Password";
        }

        public static class Logging
        {
            public const string RequestLoggingOptionsKey = nameof(Serilog) + ":" + nameof(RequestLoggingOptions);
            public const string HostLevelLogFolder = "logs/host";
            public const string HostLevelLogFile = "host.txt";
            public const string HostLevelLogTemplate = "[{UtcTimestamp} {Level:u3}] {Message:lj}{NewLine}{Exception}";

            public static class Properties
            {
                public const string UserId = nameof(UserId);
            }
        }
    }

    public static class Policy
    {
        public static class Name
        {

            public const string BackdoorUser = nameof(BackdoorUser);
            public const string GuestRestriction = nameof(GuestRestriction);
            public const string AdminOrOwner = nameof(AdminOrOwner);
            public const string AuthUser = nameof(AuthUser);
        }

        public static readonly TimeSpan GuestRestrictionMustBefore = new TimeSpan(22, 0, 0);
    }

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
        public const string SettingApi = "api/settings";
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

    public class ResultCodeResources
    {
        public static class Name
        {
            #region Common
            public const string UnknownError = nameof(UnknownError);
            public const string Success = nameof(Success);
            public const string Fail = nameof(Fail);
            public const string FailValidation = nameof(FailValidation);
            public const string NotFound = nameof(NotFound);
            public const string Unsupported = nameof(Unsupported);
            public const string DependencyDeleteFail = nameof(DependencyDeleteFail);
            public const string Unauthorized = nameof(Unauthorized);
            public const string AccessDenied = nameof(AccessDenied);
            public const string InvalidPagingRequest = nameof(InvalidPagingRequest);
            public const string InvalidSortingRequest = nameof(InvalidSortingRequest);
            public const string InvalidProjectionRequest = nameof(InvalidProjectionRequest);
            public const string EntityNotFound = nameof(EntityNotFound);
            #endregion

            #region Identity
            public const string Identity_InvalidRegisterRequest = nameof(Identity_InvalidRegisterRequest);
            public const string Identity_FailToRegisterUser = nameof(Identity_FailToRegisterUser);
            public const string Identity_FailToChangeUserRoles = nameof(Identity_FailToChangeUserRoles);
            public const string Identity_InvalidChangeUserRolesRequest = nameof(Identity_InvalidChangeUserRolesRequest);
            #endregion

            #region Setting
            public const string Setting_InvalidChangeSmtpOptionRequest = nameof(Setting_InvalidChangeSmtpOptionRequest);
            #endregion
        }
    }

    public enum ResultCode
    {
        #region Common
        [Display(Name = ResultCodeResources.Name.UnknownError)]
        UnknownError = ResultCodeGroup.Common,

        [Display(Name = ResultCodeResources.Name.Success)]
        Success = ResultCodeGroup.Common + 1,

        [Display(Name = ResultCodeResources.Name.Fail)]
        Fail = ResultCodeGroup.Common + 2,

        [Display(Name = ResultCodeResources.Name.FailValidation)]
        FailValidation = ResultCodeGroup.Common + 3,

        [Display(Name = ResultCodeResources.Name.NotFound)]
        NotFound = ResultCodeGroup.Common + 4,

        [Display(Name = ResultCodeResources.Name.Unsupported)]
        Unsupported = ResultCodeGroup.Common + 5,

        [Display(Name = ResultCodeResources.Name.DependencyDeleteFail)]
        DependencyDeleteFail = ResultCodeGroup.Common + 6,

        [Display(Name = ResultCodeResources.Name.Unauthorized)]
        Unauthorized = ResultCodeGroup.Common + 7,

        [Display(Name = ResultCodeResources.Name.AccessDenied)]
        AccessDenied = ResultCodeGroup.Common + 8,

        [Display(Name = ResultCodeResources.Name.InvalidPagingRequest)]
        InvalidPagingRequest = ResultCodeGroup.Common + 9,

        [Display(Name = ResultCodeResources.Name.InvalidSortingRequest)]
        InvalidSortingRequest = ResultCodeGroup.Common + 10,

        [Display(Name = ResultCodeResources.Name.InvalidProjectionRequest)]
        InvalidProjectionRequest = ResultCodeGroup.Common + 11,

        [Display(Name = ResultCodeResources.Name.EntityNotFound)]
        EntityNotFound = ResultCodeGroup.Common + 12,
        #endregion

        #region Identity
        [Display(Name = ResultCodeResources.Name.Identity_InvalidRegisterRequest)]
        Identity_InvalidRegisterRequest = ResultCodeGroup.Identity,

        [Display(Name = ResultCodeResources.Name.Identity_FailToRegisterUser)]
        Identity_FailToRegisterUser = ResultCodeGroup.Identity + 1,

        [Display(Name = ResultCodeResources.Name.Identity_FailToChangeUserRoles)]
        Identity_FailToChangeUserRoles = ResultCodeGroup.Identity + 2,

        [Display(Name = ResultCodeResources.Name.Identity_InvalidChangeUserRolesRequest)]
        Identity_InvalidChangeUserRolesRequest = ResultCodeGroup.Identity + 3,
        #endregion

        #region Setting
        [Display(Name = ResultCodeResources.Name.Setting_InvalidChangeSmtpOptionRequest)]
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
    }

}
