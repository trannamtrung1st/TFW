using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TFW.Cross.Models;

namespace TFW.Business.Services
{
    public interface IIdentityService
    {
        Task<GetListResponseModel<AppUserResponseModel>> GetListAppUserAsync(
            DynamicQueryAppUserModel queryModel);
    }
}
