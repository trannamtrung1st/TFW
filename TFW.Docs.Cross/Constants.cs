using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Dynamic.Core;
using System.Text;
using TFW.Docs.Cross.Models.Identity;
using TFW.Docs.Cross.Models.Setting;
using TFW.Framework.Logging.Serilog.Web;

namespace TFW.Docs.Cross
{
    public static class CachingKeys
    {
        public const string Prefix = nameof(TFW);
        public const string ListTimeZoneInfo = Prefix + ":" + nameof(ListTimeZoneInfo);
        public const string ListCultureOptions = Prefix + ":" + nameof(ListCultureOptions);
        public const string ListCurrencyOptions = Prefix + ":" + nameof(ListCurrencyOptions);
        public const string ListRegionOptions = Prefix + ":" + nameof(ListRegionOptions);
        public const string InitStatus = Prefix + ":" + nameof(InitStatus);
    }

    public static class EnvironmentVariables
    {
        public static class AspNetCoreEnv
        {
            public const string Key = "ASPNETCORE_ENVIRONMENT";
            public const string Development = nameof(Development);
        }
    }

    public static class ConfigConsts
    {
        public static class i18n
        {
            public const string ResourcesPath = "Resources";
        }

        public static class CommandLine
        {
            public const string WindowsCmd = "cmd.exe";
        }

        public static class Mail
        {
            public const string PasswordKey = "Mail:Password";
        }
    }

    public static class DynamicLinqConsts
    {
        private static ParsingConfig _defaultParsingConfig;
        public static ParsingConfig DefaultParsingConfig
        {
            get => _defaultParsingConfig; set
            {
                if (_defaultParsingConfig != null)
                    throw new InvalidOperationException($"Already initialized {nameof(DefaultParsingConfig)}");

                _defaultParsingConfig = value;
            }
        }
    }

    public static class RoleName
    {
        public static IEnumerable<string> All = new[] { Administrator };

        public const string Administrator = nameof(Administrator);
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
            public const string Identity_AlreadyInitialized = nameof(Identity_AlreadyInitialized);
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

        [Display(Name = ResultCodeResources.Name.Identity_AlreadyInitialized)]
        Identity_AlreadyInitialized = ResultCodeGroup.Identity + 4,
        #endregion

        #region Setting
        [Display(Name = ResultCodeResources.Name.Setting_InvalidChangeSmtpOptionRequest)]
        Setting_InvalidChangeSmtpOptionRequest = ResultCodeGroup.Setting,
        #endregion
    }

    public static class QueryConsts
    {
        public const char SortAscPrefix = 'a';
        public const int DefaultPage = 1;
        public const int DefaultPageLimit = 100;
    }

    public static class SecurityConsts
    {
        public const string OAuth2 = nameof(OAuth2);
        public const string ClientAuthenticationScheme = "Basic";

        public static class GrantTypes
        {
            public const string Password = "password";
            public const string RefreshToken = "refresh_token";
        }

        public static class ClaimTypes
        {
            public const string AppScope = "appscope";
            public const string ClientType = "clienttype";
        }

        public static readonly TokenValidationParameters DefaultTokenParameters;

        static SecurityConsts()
        {
            var jwtSettings = Settings.Get<JwtSettings>();
            DefaultTokenParameters = new TokenValidationParameters()
            {
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudiences = jwtSettings.Audiences,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(jwtSettings.SecretKey)),
                ClockSkew = TimeSpan.Zero
            };
        }
    }

    public enum ClientType
    {
        Confidential = 1,
        Public = 2
    }

    public static class AppClients
    {
        public static readonly ClientInfo TFWDocsWebApp = new ClientInfo
        {
            ClientId = "tfw-docs-web-app",
            ClientSecret = "ASIDOI291sad90-1248asd@!IASD1239azxitnq0707",
            Name = "TFW.Docs Web Application",
            Type = ClientType.Confidential
        };

        public static readonly IEnumerable<ClientInfo> Known = new[]
        {
            TFWDocsWebApp
        };
    }

    public class RequestDataKey
    {
        public const string PrincipalInfo = nameof(PrincipalInfo);
    }

    public static class Policy
    {
        public static class Name
        {
            public const string Admin = nameof(Admin);
            public const string AdminOrOwner = nameof(AdminOrOwner);
            public const string AuthUser = nameof(AuthUser);
        }
    }

    public static class LoggingConsts
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

    public static class Routing
    {
        public static class Root
        {
            public const string Index = "/";
            public const string Error = "/error";
            public const string Status = "/statuscode";

            public static class Post
            {
                public const string Index = "/post";
            }
        }

        public static class Admin
        {
            public const string Index = "/admin";
            public const string Error = Index + "/error";
            public const string Login = Index + "/login";
            public const string Logout = Index + "/logout";
            public const string AccessDenied = Index + "/accessdenied";
        }

        public static class Controller
        {
            public static class Auth
            {
                public const string Route = "auth";
                public const string RequestToken = "token";
            }

            public static class User
            {
                public const string Route = "api/users";
                public const string GetListAppUser = "";
                public const string GetTotalUserCount = "count";
                public const string GetListDeletedAppUser = "deleted";
                public const string GetCurrentUserProfile = "profile";
                public const string Register = "register";
                public const string Init = "init";
                public const string AddUserRoles = "user-roles";
                public const string RemoveUserRoles = "user-roles";
            }

            public static class Role
            {
                public const string Route = "api/roles";
                public const string GetAllRoles = "";
            }

            public static class Setting
            {
                public const string Route = "api/settings";
                public const string ChangeSmtpOption = "smtp";
                public const string ReloadConfiguration = "reload";
                public const string InitStatus = "init-status";
            }

            public static class Reference
            {
                public const string Route = "api/ref";
                public const string GetTimeZoneOptions = "time-zones";
                public const string GetCultureOptions = "cultures";
                public const string GetCurrencyOptions = "currencies";
                public const string GetRegionOptions = "regions";
            }

            public static class Error
            {
                public const string Route = "error";
                public const string HandleException = "";
            }
        }
    }

    public static class ApiEndpoints
    {
        public static string BaseUrl { get; set; } = "";

        public static class Auth
        {
            public static string Route => string.Join('/', BaseUrl, Routing.Controller.Auth.Route);
            public static string RequestToken => string.Join('/', Route, Routing.Controller.Auth.RequestToken);
        }

        public static class User
        {
            public static string Route => string.Join('/', BaseUrl, Routing.Controller.User.Route);
            public static string GetListAppUser => string.Join('/', Route, Routing.Controller.User.GetListAppUser);
            public static string GetTotalUserCount => string.Join('/', Route, Routing.Controller.User.GetTotalUserCount);
            public static string GetListDeletedAppUser => string.Join('/', Route, Routing.Controller.User.GetListDeletedAppUser);
            public static string GetCurrentUserProfile => string.Join('/', Route, Routing.Controller.User.GetCurrentUserProfile);
            public static string Register => string.Join('/', Route, Routing.Controller.User.Register);
            public static string Init => string.Join('/', Route, Routing.Controller.User.Init);
            public static string AddUserRoles => string.Join('/', Route, Routing.Controller.User.AddUserRoles);
            public static string RemoveUserRoles => string.Join('/', Route, Routing.Controller.User.RemoveUserRoles);
        }

        public static class Role
        {
            public static string Route => string.Join('/', BaseUrl, Routing.Controller.Role.Route);
            public static string GetAllRoles => string.Join('/', Route, Routing.Controller.Role.GetAllRoles);
        }

        public static class Setting
        {
            public static string Route => string.Join('/', BaseUrl, Routing.Controller.Setting.Route);
            public static string ChangeSmtpOption => string.Join('/', Route, Routing.Controller.Setting.ChangeSmtpOption);
            public static string ReloadConfiguration => string.Join('/', Route, Routing.Controller.Setting.ReloadConfiguration);
            public static string InitStatus => string.Join('/', Route, Routing.Controller.Setting.InitStatus);
        }

        public static class Reference
        {
            public static string Route => string.Join('/', BaseUrl, Routing.Controller.Reference.Route);
            public static string GetTimeZoneOptions => string.Join('/', Route, Routing.Controller.Reference.GetTimeZoneOptions);
            public static string GetCultureOptions => string.Join('/', Route, Routing.Controller.Reference.GetCultureOptions);
            public static string GetCurrencyOptions => string.Join('/', Route, Routing.Controller.Reference.GetCurrencyOptions);
            public static string GetRegionOptions => string.Join('/', Route, Routing.Controller.Reference.GetRegionOptions);
        }

        public static class Error
        {
            public static string Route => string.Join('/', BaseUrl, Routing.Controller.Error.Route);
            public static string HandleException => string.Join('/', Route, Routing.Controller.Error.HandleException);
        }
    }
}
