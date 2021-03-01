using System;
using System.Collections.Generic;
using TFW.Data.Core;

namespace TFW.Business.Logics
{
    public abstract class BaseLogic
    {
        protected readonly DataContext dbContext;

        public BaseLogic(DataContext dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
