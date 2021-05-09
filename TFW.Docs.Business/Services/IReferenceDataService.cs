using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TFW.Docs.Cross.Models.Common;

namespace TFW.Docs.Business.Services
{
    public interface IReferenceDataService
    {
        Task<ListResponseModel<TimeZoneOption>> GetTimeZoneOptionsAsync();
        Task<ListResponseModel<CultureOption>> GetCultureOptionsAsync();
        Task<ListResponseModel<CurrencyOption>> GetCurrencyOptionsAsync();
        Task<ListResponseModel<RegionOption>> GetRegionOptionsAsync();
    }
}
