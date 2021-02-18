using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using TFW.Cross;
using TFW.Cross.Entities;
using TFW.Cross.Models.AppUser;
using TFW.Cross.Models.Exceptions;
using TFW.Data.Repositories;
using TFW.Framework.Common;
using TFW.Framework.DI;

namespace TFW.Data.Core.Repositories
{
    [ScopedService(ServiceType = typeof(IAppUserRepository))]
    public class AppUserRepository : BaseRepository<AppUser>, IAppUserRepository
    {
        public AppUserRepository(DbContext context) : base(context)
        {
        }

        public override string EntityName => Cross.EntityName.AppUser;

        public IQueryable<AppUser> Filter(IQueryable<AppUser> query, DynamicQueryAppUserModel model)
        {
            if (model.Id != null)
                query = FilterById(query, model.Id);

            if (model.UserName != null)
                query = FilterByUsername(query, model.UserName);

            if (model.Search != null)
                query = FilterBySearch(query, model.Search);

            return query;
        }

        public IQueryable<T> Project<T>(IQueryable<AppUser> query, IEnumerable<string> fields,
            string projectionTypeName = nameof(AppUserResponseModel))
        {
            if (fields == null)
                throw AppException.From(ResultCode.InvalidSortingRequest);

            var projectionArr = fields.Select(o => DynamicQueryAppUserModel.Projections[o]).ToArray();
            var projectionStr = string.Join(',', projectionArr);

            var projectedQuery = query.Select<T>(defaultParsingConfig, $"new {projectionTypeName}({projectionStr})");

            return projectedQuery;
        }

        public IQueryable<AppUser> Sort(IQueryable<AppUser> query, IEnumerable<string> sortBy)
        {
            if (sortBy == null)
                throw AppException.From(ResultCode.InvalidSortingRequest);

            foreach (var field in sortBy)
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

        public IQueryable<AppUser> FilterById(IQueryable<AppUser> query, string id)
        {
            return query.Where(o => o.Id == id);
        }

        public IQueryable<AppUser> FilterByUsername(IQueryable<AppUser> query, string username)
        {
            return query.Where(o => o.UserName == username);
        }

        public IQueryable<AppUser> FilterBySearch(IQueryable<AppUser> query, string search)
        {
            return query.Where(o => o.UserName.Contains(search)
                || o.FullName.Contains(search));
        }
    }
}
