using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TFW.Cross.Models.Common;

namespace TFW.Business.Logics
{
    public interface IReferenceDataLogic
    {
        Task<TimeZoneOption[]> GetTimeZoneOptionsAsync();
        Task<CultureOption[]> GetCultureOptionsAsync();
        Task<CurrencyOption[]> GetCurrencyOptionsAsync();
    }
}
