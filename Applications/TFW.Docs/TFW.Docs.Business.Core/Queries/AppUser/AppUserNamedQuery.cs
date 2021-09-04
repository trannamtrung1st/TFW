using System;
using System.Collections.Generic;
using System.Linq;
using TFW.Docs.Cross.Entities;

namespace TFW.Docs.Business.Core.Queries.AppUser
{
    public static class AppUserNamedQuery
    {
        public static IQueryable<AppUserEntity> ById(this IQueryable<AppUserEntity> query, int id)
        {
            return query.Where(o => o.Id == id);
        }

        public static IQueryable<AppUserEntity> ByIds(this IQueryable<AppUserEntity> query, IEnumerable<int> ids)
        {
            return query.Where(o => ids.Contains(o.Id));
        }

        public static IQueryable<AppUserEntity> ByUsername(this IQueryable<AppUserEntity> query, string username)
        {
            return query.Where(o => o.UserName == username);
        }

        public static IQueryable<AppUserEntity> BySearchTerm(this IQueryable<AppUserEntity> query, string searchTerm)
        {
            return query.Where(o => o.UserName.Contains(searchTerm)
                || o.FullName.Contains(searchTerm));
        }

        public static IQueryable<AppUserEntity> CreatedFrom(this IQueryable<AppUserEntity> query, DateTimeOffset dateTime)
        {
            return query.Where(o => o.CreatedTime >= dateTime);
        }

        public static IQueryable<AppUserEntity> CreatedTo(this IQueryable<AppUserEntity> query, DateTimeOffset dateTime)
        {
            return query.Where(o => o.CreatedTime <= dateTime);
        }
    }
}
