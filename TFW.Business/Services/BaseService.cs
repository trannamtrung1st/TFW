using System;
using System.Collections.Generic;
using System.Text;
using TFW.Data;

namespace TFW.Business.Services
{
    public abstract class BaseService
    {
        protected readonly DataContext dataContext;

        public BaseService(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }
    }
}
