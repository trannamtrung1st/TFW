using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TFW.Docs.Cross.Entities;

namespace TFW.Docs.Business.Core.Queries
{
    public static class AppUserNamedQuery
    {
        public static IQueryable<AppUserEntity> ById(this IQueryable<AppUserEntity> query, int id)
        {
            return query.Where(o => o.Id == id);
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
    }
}
