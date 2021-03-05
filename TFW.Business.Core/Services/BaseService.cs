using System;
using System.Collections.Generic;
using System.Text;
using TFW.Data.Core;

namespace TFW.Business.Core.Services
{
    public abstract class BaseService
    {
        protected readonly DataContext dbContext;

        public BaseService(DataContext dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
