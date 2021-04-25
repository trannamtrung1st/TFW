using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TFW.Docs.Cross.Models.Common;

namespace TFW.Docs.Business.Services
{
    public interface IReferenceDataService
    {
        Task<GetListResponseModel<TimeZoneOption>> GetTimeZoneOptionsAsync();
        Task<GetListResponseModel<CultureOption>> GetCultureOptionsAsync();
        Task<GetListResponseModel<CurrencyOption>> GetCurrencyOptionsAsync();
        Task<GetListResponseModel<RegionOption>> GetRegionOptionsAsync();
    }
}
