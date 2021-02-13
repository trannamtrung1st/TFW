using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TFW.Cross.Models;
using TFW.Cross.Models.Exceptions;
using TFW.Data;

namespace TFW.Business.Logics
{
    public abstract class BaseLogic
    {
        protected readonly DataContext dataContext;

        public BaseLogic(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        protected IQueryable<T> BuildQueryPaging<T>(IQueryable<T> query, PagingQueryModel pagingModel)
        {
            if (pagingModel.Page == null)
                throw AppException.Create(error: Cross.AppError.InvalidPagingRequest);
            query = query.Skip((pagingModel.Page.Value - 1) * pagingModel.PageLimit).Take(pagingModel.PageLimit);
            return query;
        }
    }
}
