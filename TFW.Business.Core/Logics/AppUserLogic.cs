using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
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
        public AppUserLogic(DataContext dataContext) : base(dataContext)
        {
        }

        public async Task<GetListResponseModel<AppUserResponseModel>> GetListAsync(
            GetAppUserListRequestModel requestModel)
        {
            var queryModel = requestModel.MapTo<DynamicQueryAppUserModel>();
            IQueryable<AppUser> query = dataContext.Users;

            query = BuildQueryFilter(query, queryModel);
            var orgQuery = query;

            if (queryModel.Fields.IsNullOrEmpty() ||
                queryModel.Fields.Any(o => !DynamicQueryAppUserModel.Projections.ContainsKey(o)))
                throw AppException.From(ResultCode.InvalidProjectionRequest);

            if (!queryModel.SortBy.IsNullOrEmpty())
                query = BuildQuerySorting(query, queryModel);

            if (queryModel.Page > 0)
                query = BuildQueryPaging(query, queryModel);

            var projectedQuery = BuildQueryProjection<AppUserResponseModel>(query, queryModel);

            var responseModels = await projectedQuery.ToArrayAsync();
            var response = new GetListResponseModel<AppUserResponseModel>
            {
                List = responseModels,
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

        public async Task<ValidationData> ValidateGetListAsync(
            PrincipalInfo principal, GetAppUserListRequestModel requestModel)
        {
            var validationData = new ValidationData();

            if (requestModel.page < 0)
                validationData.Fail(code: Cross.ResultCode.InvalidPagingRequest);

            return await Task.FromResult(validationData);
        }

        public PrincipalInfo MapToPrincipalInfo(ClaimsPrincipal principal)
        {
            var principalInfo = principal.MapTo<PrincipalInfo>();

            return principalInfo;
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

        private IQueryable<T> BuildQueryProjection<T>(IQueryable<AppUser> query, DynamicQueryAppUserModel model,
            string projectionTypeName = nameof(AppUserResponseModel))
        {
            var projectionArr = model.Fields.Select(o => DynamicQueryAppUserModel.Projections[o]).ToArray();
            var projectionStr = string.Join(',', projectionArr);

            var projectedQuery = query.Select<T>(defaultParsingConfig, $"new {projectionTypeName}({projectionStr})");

            return projectedQuery;
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
    }
}
