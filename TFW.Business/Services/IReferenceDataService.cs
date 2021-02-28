using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TFW.Cross.Models.Common;

namespace TFW.Business.Services
{
    public interface IReferenceDataService
    {
        Task<TimeZoneOption[]> GetTimeZoneOptionsAsync();
        Task<CultureOption[]> GetCultureOptionsAsync();
    }
}
