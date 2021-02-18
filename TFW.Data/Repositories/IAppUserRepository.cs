using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TFW.Cross.Entities;
using TFW.Cross.Models.AppUser;

namespace TFW.Data.Repositories
{
    public interface IAppUserRepository : IBaseRepository<AppUser>
    {
        IQueryable<AppUser> Filter(IQueryable<AppUser> query, DynamicQueryAppUserModel model);
        IQueryable<T> Project<T>(IQueryable<AppUser> query, IEnumerable<string> fields, 
            string projectionTypeName = nameof(AppUserResponseModel));
        IQueryable<AppUser> Sort(IQueryable<AppUser> query, IEnumerable<string> sortBy);
        IQueryable<AppUser> FilterById(IQueryable<AppUser> query, string id);
        IQueryable<AppUser> FilterByUsername(IQueryable<AppUser> query, string username);
        IQueryable<AppUser> FilterBySearch(IQueryable<AppUser> query, string search);
    }
}
