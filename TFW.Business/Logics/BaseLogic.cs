using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using TFW.Cross;

namespace TFW.Business.Logics
{
    public abstract class BaseLogic
    {

        protected readonly ParsingConfig defaultParsingConfig;

        public BaseLogic()
        {
            this.defaultParsingConfig = new ParsingConfig
            {
                CustomTypeProvider = GlobalResources.CustomTypeProvider
            };
        }
    }
}
