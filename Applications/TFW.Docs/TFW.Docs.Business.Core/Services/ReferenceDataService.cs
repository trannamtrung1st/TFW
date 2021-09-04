using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using System;
using System.Linq;
using System.Threading.Tasks;
using TFW.Docs.Business.Services;
using TFW.Docs.Cross;
using TFW.Docs.Cross.Models.Common;
using TFW.Docs.Cross.Models.Setting;
using TFW.Docs.Cross.Providers;
using TFW.Docs.Data;
using TFW.Framework.AutoMapper;
using TFW.Framework.DI.Attributes;
using TFW.Framework.i18n.Helpers;

namespace TFW.Docs.Business.Core.Services
{
    [ScopedService(ServiceType = typeof(IReferenceDataService))]
    public class ReferenceDataService : BaseService, IReferenceDataService
    {
        private const int CacheDurationInHours = 1;

        private readonly IMemoryCache _memoryCache;

        public ReferenceDataService(DataContext dbContext,
            IStringLocalizer<ResultCode> resultLocalizer,
            IBusinessContextProvider contextProvider,
            IMemoryCache memoryCache) : base(dbContext, resultLocalizer, contextProvider)
        {
            _memoryCache = memoryCache;
        }

        public Task<ListResponseModel<TimeZoneOption>> GetTimeZoneOptionsAsync()
        {
            var timeZoneOptions = _memoryCache.GetOrCreate(CachingKeys.ListTimeZoneInfo,
                (entry) => TimeZoneHelper.GetAllTimeZones().MapTo<TimeZoneOption>().ToArray());

            var response = new ListResponseModel<TimeZoneOption>()
            {
                List = timeZoneOptions,
                TotalCount = timeZoneOptions.Length
            };

            return Task.FromResult(response);
        }

        public Task<ListResponseModel<CultureOption>> GetCultureOptionsAsync()
        {
            var cultureOptions = _memoryCache.GetOrCreate(CachingKeys.ListCultureOptions,
                (entry) =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(CacheDurationInHours);
                    return Settings.Get<AppSettings>().SupportedCultureInfos.MapTo<CultureOption>().ToArray();
                });

            var response = new ListResponseModel<CultureOption>()
            {
                List = cultureOptions,
                TotalCount = cultureOptions.Length
            };

            return Task.FromResult(response);
        }

        public Task<ListResponseModel<CurrencyOption>> GetCurrencyOptionsAsync()
        {
            var currencyOptions = _memoryCache.GetOrCreate(CachingKeys.ListCurrencyOptions,
                (entry) =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(CacheDurationInHours);
                    return Settings.Get<AppSettings>().SupportedRegionInfos.MapTo<CurrencyOption>().ToArray();
                });

            var response = new ListResponseModel<CurrencyOption>()
            {
                List = currencyOptions,
                TotalCount = currencyOptions.Length
            };

            return Task.FromResult(response);
        }

        public Task<ListResponseModel<RegionOption>> GetRegionOptionsAsync()
        {
            var countryOptions = _memoryCache.GetOrCreate(CachingKeys.ListRegionOptions,
                (entry) => CultureHelper.GetDistinctRegions().MapTo<RegionOption>().ToArray());

            var response = new ListResponseModel<RegionOption>()
            {
                List = countryOptions,
                TotalCount = countryOptions.Length
            };

            return Task.FromResult(response);
        }

        #region Preparation
        #endregion
    }
}
