using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TFW.Cross.Models.Common;

namespace TFW.Business.Logics
{
    public interface IReferenceDataLogic
    {
        Task<GetListResponseModel<TimeZoneOption>> GetTimeZoneOptionsAsync();
        Task<GetListResponseModel<CultureOption>> GetCultureOptionsAsync();
        Task<GetListResponseModel<CurrencyOption>> GetCurrencyOptionsAsync();
        Task<GetListResponseModel<RegionOption>> GetRegionOptionsAsync();
    }
}
