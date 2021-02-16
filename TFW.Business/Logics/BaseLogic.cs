using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using TFW.Cross;
using TFW.Cross.Models;
using TFW.Cross.Models.Exceptions;
using TFW.Data;

namespace TFW.Business.Logics
{
    public abstract class BaseLogic
    {
        protected readonly DataContext dataContext;
        protected readonly ParsingConfig defaultParsingConfig;

        public BaseLogic(DataContext dataContext)
        {
            this.dataContext = dataContext;
            this.defaultParsingConfig = new ParsingConfig
            {
                CustomTypeProvider = GlobalResources.CustomTypeProvider
            };
        }

        protected IQueryable<T> BuildQueryPaging<T>(IQueryable<T> query, PagingQueryModel pagingModel)
        {
            if (pagingModel.Page <= 0)
                throw AppException.From(Cross.ResultCode.InvalidPagingRequest);
            
            query = query.Skip((pagingModel.Page - 1) * pagingModel.PageLimit).Take(pagingModel.PageLimit);
            
            return query;
        }
    }
}
