using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Dynamic.Core.CustomTypeProviders;
using System.Security.Claims;
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
using TFW.Framework.DI;

namespace TFW.Business.Core.Logics
{
    [ScopedService(ServiceType = typeof(IAppUserLogic))]
    public class AppUserLogic : BaseLogic, IAppUserLogic
    {
        public AppUserLogic(DataContext dataContext, IDynamicLinkCustomTypeProvider dynamicLinkCustomTypeProvider)
            : base(dataContext, dynamicLinkCustomTypeProvider)
        {
        }

        #region AppUser
        public async Task<GetListResponseModel<AppUserResponseModel>> GetListAppUserAsync(
            GetAppUserListRequestModel requestModel)
        {
            var queryModel = GlobalResources.Mapper.MapTo<DynamicQueryAppUserModel>(requestModel);
            IQueryable<AppUser> query = dataContext.Users;
            query = BuildQueryFilter(query, queryModel);
            var orgQuery = query;
            if (queryModel.Fields.IsNullOrEmpty() ||
                queryModel.Fields.Any(o => !DynamicQueryAppUserModel.Projections.ContainsKey(o)))
                throw AppException.From(ResultCode.InvalidProjectionRequest);
            query = BuildQueryProjection(query, queryModel);
            if (!queryModel.SortBy.IsNullOrEmpty())
                query = BuildQuerySorting(query, queryModel);
            if (queryModel.Page > 0)
                query = BuildQueryPaging(query, queryModel);
            var entities = await query.ToArrayAsync();
            var responseList = GlobalResources.Mapper.MapTo<AppUserResponseModel>(entities).ToArray();
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

        public async Task<ValidationData> ValidateGetAppUserListAsync(
            ClaimsPrincipal principal, GetAppUserListRequestModel requestModel)
        {
            var principalInfo = GlobalResources.Mapper.MapTo<PrincipalInfo>(principal);
            var validationData = new ValidationData();
            if (requestModel.page < 0)
                validationData.Fail(code: Cross.ResultCode.InvalidPagingRequest);
            return await Task.FromResult(validationData);
        }

        private IQueryable<AppUser> BuildQueryFilter(IQueryable<AppUser> query, DynamicQueryAppUserModel model)
        {
            if (model.Id != null)
                query = BuildQueryById(query, model.Id);
            if (model.UserName != null)
                query = BuildQueryByUsername(query, model.UserName);
            if (model.Search != null)
                query = BuildQueryBySearch(query, model.Search);
            return query;
        }

        private IQueryable<AppUser> BuildQueryProjection(IQueryable<AppUser> query, DynamicQueryAppUserModel model)
        {
            var projectionArr = model.Fields.Select(o => DynamicQueryAppUserModel.Projections[o]).ToArray();
            var projectionStr = string.Join(',', projectionArr);
            query = query.Select<AppUser>(defaultParsingConfig, $"new ({projectionStr})");
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
                    case DynamicQueryAppUserModel.SortByUsername:
                        {
                            if (asc)
                                query = query.SequentialOrderBy(o => o.UserName);
                            else
                                query = query.SequentialOrderByDesc(o => o.UserName);
                        }
                        break;
                    default:
                        throw AppException.From(ResultCode.InvalidPagingRequest);
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

        private IQueryable<AppUser> BuildQueryBySearch(IQueryable<AppUser> query, string search)
        {
            return query.Where(o => o.UserName.Contains(search)
                || o.FullName.Contains(search));
        }
        #endregion
    }
}
