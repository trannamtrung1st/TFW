using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TFW.Docs.Cross.Entities;

namespace TFW.Docs.Business.Core.Queries
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

        public static IQueryable<AppUser> BySearchTerm(this IQueryable<AppUser> query, string searchTerm)
        {
            return query.Where(o => o.UserName.Contains(searchTerm)
                || o.FullName.Contains(searchTerm));
        }
    }
}
