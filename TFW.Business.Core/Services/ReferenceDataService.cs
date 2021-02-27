using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TFW.Business.Logics;
using TFW.Business.Services;
using TFW.Cross.Models.Common;
using TFW.Framework.DI.Attributes;

namespace TFW.Business.Core.Services
{
    [ScopedService(ServiceType = typeof(IReferenceDataService))]
    public class ReferenceDataService : BaseService, IReferenceDataService
    {
        private readonly IReferenceDataLogic _referenceDataLogic;

        public ReferenceDataService(IReferenceDataLogic referenceDataLogic)
        {
            _referenceDataLogic = referenceDataLogic;
        }

        public Task<TimeZoneOption[]> GetTimeZoneOptionsAsync()
        {
            return _referenceDataLogic.GetTimeZoneOptionsAsync();
        }
    }
}
