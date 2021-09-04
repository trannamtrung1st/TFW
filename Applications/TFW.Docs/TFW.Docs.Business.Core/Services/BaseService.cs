using Microsoft.Extensions.Localization;
using TFW.Docs.Cross;
using TFW.Docs.Cross.Providers;
using TFW.Docs.Data;

namespace TFW.Docs.Business.Core.Services
{
    public abstract class BaseService
    {
        protected readonly DataContext dbContext;
        protected readonly IStringLocalizer resultLocalizer;
        protected readonly IBusinessContextProvider contextProvider;

        public BaseService(DataContext dbContext,
            IStringLocalizer<ResultCode> resultLocalizer,
            IBusinessContextProvider contextProvider)
        {
            this.dbContext = dbContext;
            this.resultLocalizer = resultLocalizer;
            this.contextProvider = contextProvider;
        }
    }
}
