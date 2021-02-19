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
