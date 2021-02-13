using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFW.Cross.Entities;
using TFW.Cross.Models;

namespace TFW.Business.Logics
{
    public interface IAppUserLogic : ILogic
    {
        Task<GetListResponseModel<AppUserResponseModel>> GetListAppUserAsync(
            DynamicQueryAppUserModel queryModel);
        IQueryable<AppUser> QueryById(string id);
        IQueryable<AppUser> QueryByUsername(string username);
    }
}
