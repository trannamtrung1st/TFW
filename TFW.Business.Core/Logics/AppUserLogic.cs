using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using TFW.Business.Logics;
using TFW.Cross;
using TFW.Cross.Entities;
using TFW.Cross.Models;
using TFW.Cross.Models.Exceptions;
using TFW.Data;
using TFW.Framework.AutoMapper;
using TFW.Framework.Common;

namespace TFW.Business.Core.Logics
{
    public class AppUserLogic : BaseLogic, IAppUserLogic
    {
        public AppUserLogic(DataContext dataContext) : base(dataContext)
        {
        }

        #region AppUser
        public async Task<GetListResponseModel<AppUserResponseModel>> GetListAppUserAsync(
            DynamicQueryAppUserModel queryModel)
        {
            IQueryable<AppUser> query = dataContext.Users;
            query = BuildQueryFilter(query, queryModel);
            var orgQuery = query;
            if (queryModel.Fields != null)
                query = BuildQueryProjection(query, queryModel);
            if (queryModel.SortBy != null)
                query = BuildQuerySorting(query, queryModel);
            if (queryModel.Page > 0)
                query = BuildQueryPaging(query, queryModel);
            var entities = await query.ToArrayAsync();
            var responseList = entities.To<AppUserResponseModel>().ToArray();
            var response = new GetListResponseModel<AppUserResponseModel>
            {
                List = responseList,
            };
            if (queryModel.CountTotal)
                response.TotalCount = await orgQuery.CountAsync();
            return response;
        }

        public IQueryable<AppUser> QueryById(string id)
        {
            IQueryable<AppUser> query = dataContext.Users;
            query = BuildQueryById(query, id);
            return query;
        }

        public IQueryable<AppUser> QueryByUsername(string username)
        {
            IQueryable<AppUser> query = dataContext.Users;
            query = BuildQueryByUsername(query, username);
            return query;
        }

        private IQueryable<AppUser> BuildQueryFilter(IQueryable<AppUser> query, DynamicQueryAppUserModel model)
        {
            if (model.Id != null)
                query = BuildQueryById(query, model.Id);
            if (model.UserName != null)
                query = BuildQueryByUsername(query, model.UserName);
            return query;
        }

        private IQueryable<AppUser> BuildQueryProjection(IQueryable<AppUser> query, DynamicQueryAppUserModel model)
        {
            var fieldStr = model.Fields.ToCommaString();
            query = query.Select<AppUser>($"new ({fieldStr})");
            return query;
        }

        private IQueryable<AppUser> BuildQuerySorting(IQueryable<AppUser> query, DynamicQueryAppUserModel model)
        {
            foreach (var field in model.SortBy)
            {
                var asc = field[0] == QueryConsts.SortAscPrefix;
                var fieldName = field.Remove(0, 1);
                switch (fieldName)
                {
                    case AppUserQueryConsts.SortByUsername:
                        {
                            if (asc)
                                query = query.SequentialOrderBy(o => o.UserName);
                            else
                                query = query.SequentialOrderByDesc(o => o.UserName);
                        }
                        break;
                    default:
                        throw AppException.Create(error: AppError.InvalidAppUserSorting);
                }
            }
            return query;
        }

        private IQueryable<AppUser> BuildQueryById(IQueryable<AppUser> query, string id)
        {
            return query.Where(o => o.Id == id);
        }

        private IQueryable<AppUser> BuildQueryByUsername(IQueryable<AppUser> query, string username)
        {
            return query.Where(o => o.UserName == username);
        }
        #endregion
    }
}
