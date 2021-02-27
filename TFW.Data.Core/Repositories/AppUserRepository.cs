﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using TFW.Cross.Entities;
using TFW.Data.Repositories;
using TFW.Framework.DI.Attributes;
using TFW.Framework.EFCore;
using TFW.Framework.EFCore.Repository;

namespace TFW.Data.Core.Repositories
{
    [ScopedService(ServiceType = typeof(IAppUserRepository))]
    public class AppUserRepository : BaseRepository<AppUser, DataContext>, IAppUserRepository
    {
        public AppUserRepository(DataContext context) : base(context)
        {
        }

        public override string EntityTableName => Cross.EntityTableName.AppUser;

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
