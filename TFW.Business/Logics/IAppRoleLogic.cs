using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TFW.Cross.Models.AppRole;
using TFW.Cross.Models.Common;

namespace TFW.Business.Logics
{
    public interface IAppRoleLogic
    {
        Task<GetListResponseModel<GetListRolesResponseModel>> GetListAsync();
    }
}
