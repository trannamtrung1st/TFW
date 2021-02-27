using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFW.Business.Logics;
using TFW.Cross.Models.Common;
using TFW.Framework.AutoMapper.Helpers;
using TFW.Framework.DI.Attributes;
using TFW.Framework.i18n.Helpers;

namespace TFW.Business.Core.Logics
{
    [ScopedService(ServiceType = typeof(IReferenceDataLogic))]
    public class ReferenceDataLogic : BaseLogic, IReferenceDataLogic
    {
        public ReferenceDataLogic() : base()
        {
        }

        public Task<TimeZoneOption[]> GetTimeZoneOptionsAsync()
        {
            // [TODO] add caching
            var timeZoneOptions = TimeZoneHelper.GetAllTimeZones().MapTo<TimeZoneOption>().ToArray();

            return Task.FromResult(timeZoneOptions);
        }
    }
}
