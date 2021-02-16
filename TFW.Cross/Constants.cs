using System;
using System.Collections.Generic;
using System.ComponentModel;
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

    public enum ResultCode
    {
        [Description("Unknown error")]
        UnknownError = 1,
        
        [Description("Success")]
        Success = 2,
        
        [Description("Fail")]
        Fail = 3,
        
        [Description("Fail validation")]
        FailValidation = 4,
        
        [Description("Not found")]
        NotFound = 5,
        
        [Description("Unsupported")]
        Unsupported = 6,
        
        [Description("Can not delete because of dependencies")]
        DependencyDeleteFail = 7,
        
        [Description("Unauthorized")]
        Unauthorized = 8,
        
        [Description("Access denied")]
        AccessDenied = 9,
        
        [Description("Invalid paging request")]
        InvalidPagingRequest = 10,
        
        [Description("Invalid sorting request")]
        InvalidSortingRequest = 11,
        
        [Description("Invalid projection request")]
        InvalidProjectionRequest = 12,
    }

}
