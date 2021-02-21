using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TFW.Cross.Entities;
using TFW.Framework.EFCore.Repository;

namespace TFW.Data.Repositories
{
    public interface IAppUserRepository : IBaseRepository<AppUser>
    {
        IQueryable<AppUser> FilterById(IQueryable<AppUser> query, string id);
        IQueryable<AppUser> FilterByUsername(IQueryable<AppUser> query, string username);
        IQueryable<AppUser> FilterBySearch(IQueryable<AppUser> query, string search);
        IQueryable<AppUser> FilterDeleted(IQueryable<AppUser> query);
    }
}
