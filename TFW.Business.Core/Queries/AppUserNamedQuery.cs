using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TFW.Cross.Entities;

namespace TFW.Business.Core.Queries
{
    public static class AppUserNamedQuery
    {
        public static IQueryable<AppUser> ById(this IQueryable<AppUser> query, string id)
        {
            return query.Where(o => o.Id == id);
        }

        public static IQueryable<AppUser> ByUsername(this IQueryable<AppUser> query, string username)
        {
            return query.Where(o => o.UserName == username);
        }

        public static IQueryable<AppUser> BySearch(this IQueryable<AppUser> query, string search)
        {
            return query.Where(o => o.UserName.Contains(search)
                || o.FullName.Contains(search));
        }
    }
}
