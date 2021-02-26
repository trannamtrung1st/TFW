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
using TFW.Framework.AutoMapper.Helpers;
using TFW.Framework.Common.Helpers;
using TFW.Framework.DI.Attributes;

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
            GetAppUserListRequestModel requestModel, Type projectionType = null)
        {
            #region Validation
            var userInfo = BusinessContext.Current?.PrincipalInfo;
            var validationData = new ValidationData();

            // validation logic here

            if (!validationData.IsValid)
                throw AppValidationException.From(validationData);
            #endregion

            var queryModel = requestModel.MapTo<DynamicQueryAppUserModel>();
            IQueryable<AppUser> query = _appUserRepository.AsNoTracking();

            #region Filter
            if (queryModel.Id != null)
                query = _appUserRepository.FilterById(query, queryModel.Id);

            if (queryModel.UserName != null)
                query = _appUserRepository.FilterByUsername(query, queryModel.UserName);

            if (queryModel.Search != null)
                query = _appUserRepository.FilterBySearch(query, queryModel.Search);
            #endregion

            var orgQuery = query;

            #region Sorting
            if (!queryModel.SortBy.IsNullOrEmpty())
            {
                foreach (var field in queryModel.SortBy)
                {
                    var asc = field[0] == QueryConsts.SortAscPrefix;
                    var fieldName = field.Remove(0, 1);

                    switch (fieldName)
                    {
                        case DynamicQueryAppUserModel.SortByUsername:
                            {
                                if (asc)
                                    query = query.SequentialOrderBy(o => o.UserName);
                                else
                                    query = query.SequentialOrderByDesc(o => o.UserName);
                            }
                            break;
                        default:
                            throw AppException.From(ResultCode.InvalidPagingRequest);
                    }
                }
            }
            #endregion

            if (queryModel.Page > 0)
                query = _appUserRepository.Limit(query, queryModel.Page, queryModel.PageLimit);

            #region Projection
            var projectionArr = queryModel.Fields.Select(o => DynamicQueryAppUserModel.Projections[o]).ToArray();
            var projectionStr = string.Join(',', projectionArr);
            if (projectionType == null) projectionType = typeof(AppUserResponseModel);

            var projectedQuery = query.Select<AppUserResponseModel>(defaultParsingConfig,
                $"new {projectionType.FullName}({projectionStr})");
            #endregion

            var responseModels = await projectedQuery.ToArrayAsync();
            var response = new GetListResponseModel<AppUserResponseModel>
            {
                List = responseModels,
            };

            if (queryModel.CountTotal)
                response.TotalCount = await orgQuery.CountAsync();

            return response;
        }

        public async Task<GetListResponseModel<AppUserResponseModel>> GetListDeletedAppUserAsync()
        {
            var query = _appUserRepository.AsNoTracking();

            var responseModels = await _appUserRepository.FilterDeleted(query)
                .DefaultProjectTo<AppUserResponseModel>().ToArrayAsync();

            var response = new GetListResponseModel<AppUserResponseModel>
            {
                List = responseModels,
                TotalCount = responseModels.Length
            };

            return response;
        }

        public PrincipalInfo MapToPrincipalInfo(ClaimsPrincipal principal)
        {
            var principalInfo = principal.MapTo<PrincipalInfo>();

            return principalInfo;
        }
    }
}
