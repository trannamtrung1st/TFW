using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TFW.Cross
{
    public static class AppUserQueryConsts
    {
        public const string SortByUsername = "username";
        public const string DefaultSortBy = "a" + SortByUsername;
    }

    public static class QueryConsts
    {
        public const char SortAscPrefix = 'a';
        public const int DefaultPageLimit = 100;
    }

    public static class ApiEndpoint
    {
        public const string Error = "error";
    }

    public enum AppError
    {
        [Description("Invalid paging request")]
        InvalidPagingRequest = 1,
        [Description("Invalid app user sorting")]
        InvalidAppUserSorting = 2
    }
}
