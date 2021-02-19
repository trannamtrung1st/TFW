using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TFW.Framework.i18n;

namespace TFW.Cross
{
    public static class QueryConsts
    {
        public const char SortAscPrefix = 'a';
        public const int DefaultPageLimit = 100;
    }

    public static class ApiEndpoint
    {
        public const string UserApi = "api/users";
        public const string Error = "error";
    }

    public static class TimeZoneConsts
    {
        public static readonly IDictionary<string, TimeZoneInfo> TimezoneMap;

        static TimeZoneConsts()
        {
            TimezoneMap = TimezoneHelper.GetIsoToTimeZoneMapping();
        }
    }

    public static class ResultCodeGroup
    {
        public const int Common = 0;

        public const int AppUser = 0;
    }

    public enum ResultCode
    {
        #region Common
        [Display(Name = "Unknown error")]
        UnknownError = ResultCodeGroup.Common + 1,

        [Display(Name = "Success")]
        Success = ResultCodeGroup.Common + 2,

        [Display(Name = "Fail")]
        Fail = ResultCodeGroup.Common + 3,

        [Display(Name = "Fail validation")]
        FailValidation = ResultCodeGroup.Common + 4,

        [Display(Name = "Not found")]
        NotFound = ResultCodeGroup.Common + 5,

        [Display(Name = "Unsupported")]
        Unsupported = ResultCodeGroup.Common + 6,

        [Display(Name = "Can not delete because of dependencies")]
        DependencyDeleteFail = ResultCodeGroup.Common + 7,

        [Display(Name = "Unauthorized")]
        Unauthorized = ResultCodeGroup.Common + 8,

        [Display(Name = "Access denied")]
        AccessDenied = ResultCodeGroup.Common + 9,

        [Display(Name = "Invalid paging request")]
        InvalidPagingRequest = ResultCodeGroup.Common + 10,

        [Display(Name = "Invalid sorting request")]
        InvalidSortingRequest = ResultCodeGroup.Common + 11,

        [Display(Name = "Invalid projection request")]
        InvalidProjectionRequest = ResultCodeGroup.Common + 12,

        [Display(Name = "Entity not found")]
        EntityNotFound = ResultCodeGroup.Common + 13,
        #endregion

        #region AppUser
        #endregion
    }

    public static class EntityName
    {
        public const string AppUser = "AspNetUsers";
        public const string AppRole = "AspNetRoles";
        public const string AppUserRole = "AspNetUserRoles";
        public const string Note = nameof(Entities.Note);
        public const string NoteCategory = nameof(Entities.NoteCategory);
    }

}
