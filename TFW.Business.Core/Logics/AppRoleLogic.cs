using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFW.Business.Logics;
using TFW.Cross;
using TFW.Cross.Entities;
using TFW.Cross.Models.AppRole;
using TFW.Cross.Models.Common;
using TFW.Cross.Models.Exceptions;
using TFW.Data.Core;
using TFW.Framework.AutoMapper.Helpers;
using TFW.Framework.DI.Attributes;

namespace TFW.Business.Core.Logics
{
    [ScopedService(ServiceType = typeof(IAppRoleLogic))]
    public class AppRoleLogic : BaseLogic, IAppRoleLogic
    {
        public AppRoleLogic(DataContext dbContext) : base(dbContext)
        {
        }

        public async Task<GetListResponseModel<GetListRolesResponseModel>> GetListAsync()
        {
            #region Validation
            var userInfo = BusinessContext.Current?.PrincipalInfo;
            var validationData = new ValidationData();

            // validation logic here

            if (!validationData.IsValid)
                throw AppValidationException.From(validationData);
            #endregion

            var query = dbContext.Roles.AsNoTracking()
                .OrderBy(o => o.Name);

            var responseModels = await query.DefaultProjectTo<GetListRolesResponseModel>().ToArrayAsync();

            var response = new GetListResponseModel<GetListRolesResponseModel>
            {
                List = responseModels,
                TotalCount = await query.CountAsync()
            };

            return response;
        }
    }
}
