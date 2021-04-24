using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFW.Business.Services;
using TFW.Cross.Models.Common;
using TFW.Cross.Models.Setting;
using TFW.Docs.Data;
using TFW.Framework.AutoMapper;
using TFW.Framework.DI.Attributes;
using TFW.Framework.i18n.Helpers;

namespace TFW.Business.Core.Services
{
    [ScopedService(ServiceType = typeof(IReferenceDataService))]
    public class ReferenceDataService : BaseService, IReferenceDataService
    {
        public ReferenceDataService(DataContext dbContext) : base(dbContext)
        {
        }

        public Task<GetListResponseModel<TimeZoneOption>> GetTimeZoneOptionsAsync()
        {
            // [TODO] add caching
            var timeZoneOptions = TimeZoneHelper.GetAllTimeZones().MapTo<TimeZoneOption>().ToArray();

            var response = new GetListResponseModel<TimeZoneOption>()
            {
                List = timeZoneOptions,
                TotalCount = timeZoneOptions.Length
            };

            return Task.FromResult(response);
        }

        public Task<GetListResponseModel<CultureOption>> GetCultureOptionsAsync()
        {
            // [TODO] add caching
            var cultureOptions = Settings.App.SupportedCultureInfos.MapTo<CultureOption>().ToArray();

            var response = new GetListResponseModel<CultureOption>()
            {
                List = cultureOptions,
                TotalCount = cultureOptions.Length
            };

            return Task.FromResult(response);
        }

        public Task<GetListResponseModel<CurrencyOption>> GetCurrencyOptionsAsync()
        {
            // [TODO] add caching
            var currencyOptions = Settings.App.SupportedRegionInfos.MapTo<CurrencyOption>().ToArray();

            var response = new GetListResponseModel<CurrencyOption>()
            {
                List = currencyOptions,
                TotalCount = currencyOptions.Length
            };

            return Task.FromResult(response);
        }

        public Task<GetListResponseModel<RegionOption>> GetRegionOptionsAsync()
        {
            // [TODO] add caching
            var countryOptions = CultureHelper.GetDistinctRegions().MapTo<RegionOption>().ToArray();

            var response = new GetListResponseModel<RegionOption>()
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
