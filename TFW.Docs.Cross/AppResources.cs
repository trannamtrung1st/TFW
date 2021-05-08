using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Docs.Cross
{
    public class AppResources
    {
        public static class ResultCode
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
                public const string Identity_InvalidRedirectUrl = nameof(Identity_InvalidRedirectUrl);
                #endregion

                #region Setting
                public const string Setting_InvalidChangeSmtpOptionRequest = nameof(Setting_InvalidChangeSmtpOptionRequest);
                #endregion

                #region PostCategory
                public const string PostCategory_InvalidCreatePostCategoryRequest = nameof(PostCategory_InvalidCreatePostCategoryRequest);
                public const string PostCategory_InvalidCreatePostCategoryLocalizationRequest = nameof(PostCategory_InvalidCreatePostCategoryLocalizationRequest);
                public const string PostCategory_LocalizationExists = nameof(PostCategory_LocalizationExists);
                public const string PostCategory_InvalidUpdateLocalizationsRequest = nameof(PostCategory_InvalidUpdateLocalizationsRequest);
                #endregion
            }
        }

        public static class Validators
        {
            public static class AppUser
            {
                public static class ChangeUserRolesBaseModelValidator
                {
                    public const string InvalidRoleName = nameof(InvalidRoleName);
                }
            }

            public static class Identity
            {
                public static class RegisterModelValidator
                {
                    public const string ConfirmPasswordDoesNotMatch = nameof(ConfirmPasswordDoesNotMatch);
                }
            }
        }
    }
}
