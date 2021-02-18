using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TFW.Business.Logics;
using TFW.Cross;
using TFW.Cross.Entities;
using TFW.Cross.Models.AppUser;
using TFW.Cross.Models.Common;
using TFW.Cross.Models.Exceptions;
using TFW.Data.Repositories;
using TFW.Framework.AutoMapper;
using TFW.Framework.Common;
using TFW.Framework.DI;

namespace TFW.Business.Core.Logics
{
    [ScopedService(ServiceType = typeof(IAppUserLogic))]
    public class AppUserLogic : BaseLogic, IAppUserLogic
    {
        private readonly IAppUserRepository _appUserRepository;

        public AppUserLogic(IAppUserRepository appUserRepository) : base()
        {
            _appUserRepository = appUserRepository;
        }

        public async Task<GetListResponseModel<AppUserResponseModel>> GetListAsync(
            GetAppUserListRequestModel requestModel)
        {
            #region Validation
            var userInfo = PrincipalInfo.Current;
            var validationData = new ValidationData();

            if (requestModel.page < 0)
                validationData.Fail(code: Cross.ResultCode.InvalidPagingRequest);

            if (!validationData.IsValid)
                throw AppValidationException.From(validationData);
            #endregion

            var queryModel = requestModel.MapTo<DynamicQueryAppUserModel>();
            IQueryable<AppUser> query = _appUserRepository.AsNoTracking();

            query = _appUserRepository.Filter(query, queryModel);
            var orgQuery = query;

            if (queryModel.Fields.IsNullOrEmpty() ||
                queryModel.Fields.Any(o => !DynamicQueryAppUserModel.Projections.ContainsKey(o)))
                throw AppException.From(ResultCode.InvalidProjectionRequest);

            if (!queryModel.SortBy.IsNullOrEmpty())
                query = _appUserRepository.Sort(query, queryModel.SortBy);

            if (queryModel.Page > 0)
                query = _appUserRepository.Limit(query, queryModel);

            var projectedQuery = _appUserRepository.Project<AppUserResponseModel>(query, queryModel.Fields);

            var responseModels = await projectedQuery.ToArrayAsync();
            var response = new GetListResponseModel<AppUserResponseModel>
            {
                List = responseModels,
            };

            if (queryModel.CountTotal)
                response.TotalCount = await orgQuery.CountAsync();

            return response;
        }

        public PrincipalInfo MapToPrincipalInfo(ClaimsPrincipal principal)
        {
            var principalInfo = principal.MapTo<PrincipalInfo>();

            return principalInfo;
        }
    }
}
