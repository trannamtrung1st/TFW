using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Dynamic.Core.CustomTypeProviders;
using System.Text;
using TFW.Cross.Models;
using TFW.Cross.Models.Exceptions;
using TFW.Data;

namespace TFW.Business.Logics
{
    public abstract class BaseLogic
    {
        protected readonly DataContext dataContext;
        protected readonly IDynamicLinkCustomTypeProvider dynamicLinkCustomTypeProvider;
        protected readonly ParsingConfig defaultParsingConfig;

        public BaseLogic(DataContext dataContext, IDynamicLinkCustomTypeProvider dynamicLinkCustomTypeProvider)
        {
            this.dataContext = dataContext;
            this.dynamicLinkCustomTypeProvider = dynamicLinkCustomTypeProvider;
            this.defaultParsingConfig = new ParsingConfig
            {
                CustomTypeProvider = dynamicLinkCustomTypeProvider
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
