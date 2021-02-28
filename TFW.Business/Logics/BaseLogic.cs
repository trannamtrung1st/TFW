using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using TFW.Cross;
using TFW.Data.Core;

namespace TFW.Business.Logics
{
    public abstract class BaseLogic
    {

        protected readonly ParsingConfig defaultParsingConfig;
        protected readonly DataContext dbContext;

        public BaseLogic(DataContext dbContext)
        {
            this.dbContext = dbContext;
            this.defaultParsingConfig = new ParsingConfig
            {
                CustomTypeProvider = GlobalResources.CustomTypeProvider
            };
        }
    }
}
