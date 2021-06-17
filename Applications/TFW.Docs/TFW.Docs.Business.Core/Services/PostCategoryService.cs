using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using TFW.Docs.Business.Core.Queries;
using TFW.Docs.Business.Core.Queries.PostCategory;
using TFW.Docs.Business.Services;
using TFW.Docs.Cross;
using TFW.Docs.Cross.Entities;
using TFW.Docs.Cross.Exceptions;
using TFW.Docs.Cross.Helpers;
using TFW.Docs.Cross.Models.Common;
using TFW.Docs.Cross.Models.PostCategory;
using TFW.Docs.Cross.Providers;
using TFW.Docs.Data;
using TFW.Framework.AutoMapper;
using TFW.Framework.Common.Extensions;
using TFW.Framework.DI.Attributes;
using TFW.Framework.EFCore.Query;

namespace TFW.Docs.Business.Core.Services
{
    [ScopedService(ServiceType = typeof(IPostCategoryService))]
    public class PostCategoryService : BaseService, IPostCategoryService
    {
        public PostCategoryService(DataContext dbContext,
            IStringLocalizer<ResultCode> resultLocalizer,
            IBusinessContextProvider contextProvider) : base(dbContext, resultLocalizer, contextProvider)
        {
        }


        public async Task<ListResponseModel<TModel>> GetListPostCategoryAsync<TModel>(
            ListPostCategoryRequestModel requestModel, ParsingConfig parsingConfig = null)
        {
            #region Validation
            var userInfo = contextProvider.BusinessContext.PrincipalInfo;
            var validationData = new ValidationData(resultLocalizer);

            // validation logic here

            if (!validationData.IsValid)
                throw validationData.BuildException();
            #endregion

            IQueryable<PostCategoryEntity> query = dbContext.PostCategory.AsNoTracking();
            IQueryable<ListPostCategoryJoinModel> joinedQuery;

            #region Filter
            if (requestModel.Ids?.Any() == true)
                query = query.ByIds(requestModel.Ids);

            string lang = requestModel.Lang;
            string region = requestModel.Region;
            if (string.IsNullOrEmpty(lang))
                lang = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;

            var locQuery = dbContext.PostCategoryLocalization.ByCulture(lang, region);

            joinedQuery = from pc in query
                          from pcl in locQuery.Where(o => o.EntityId == pc.Id).Take(1).DefaultIfEmpty()
                          select new ListPostCategoryJoinModel
                          {
                              CreatedTime = pc.CreatedTime,
                              Description = pcl.Description,
                              Id = pc.Id,
                              Lang = pcl.Lang,
                              Region = pcl.Region,
                              Title = pcl.Title
                          };

            if (!string.IsNullOrWhiteSpace(requestModel.SearchTerm))
                joinedQuery = joinedQuery.BySearchTerm(requestModel.SearchTerm);
            #endregion

            var orgQuery = joinedQuery;

            #region Sorting
            var sortByArr = requestModel.GetSortByArr();
            if (!sortByArr.IsNullOrEmpty())
            {
                foreach (var field in sortByArr)
                {
                    var asc = field[0] == QueryConsts.SortAscPrefix;
                    var fieldName = field.Remove(0, 1);

                    switch (fieldName)
                    {
                        case ListPostCategoryRequestModel.SortByTitle:
                            {
                                if (asc)
                                    joinedQuery = joinedQuery.SequentialOrderBy(o => o.Title);
                                else
                                    joinedQuery = joinedQuery.SequentialOrderByDesc(o => o.Title);
                            }
                            break;
                        default:
                            throw AppValidationException.From(resultLocalizer, ResultCode.InvalidPagingRequest);
                    }
                }
            }
            #endregion

            if (requestModel.Page > 0)
                joinedQuery = joinedQuery.Limit(requestModel.Page, requestModel.PageLimit);

            #region Projection
            var projectionArr = requestModel.GetFieldsArr().Select(o => ListPostCategoryRequestModel.Projections[o]).ToArray();
            var projectionStr = string.Join(',', projectionArr);

            var projectedQuery = joinedQuery.Select<TModel>(
                parsingConfig ?? DynamicLinqConsts.DefaultParsingConfig,
                $"new {typeof(TModel).FullName}({projectionStr})");
            #endregion

            var responseModels = await projectedQuery.ToArrayAsync();
            var response = new ListResponseModel<TModel>
            {
                List = responseModels,
            };

            if (requestModel.CountTotal)
                response.TotalCount = await orgQuery.CountAsync();

            return response;
        }

        public async Task<PostCategoryDetailModel> GetPostCategoryDetailAsync(PostCategoryDetailRequestModel model)
        {
            #region Validation
            var userInfo = contextProvider.BusinessContext.PrincipalInfo;
            var validationData = new ValidationData(resultLocalizer);

            // validation logic here

            if (!validationData.IsValid)
                throw validationData.BuildException();
            #endregion

            var detailModel = await dbContext.PostCategory.ById(model.Id)
                .DefaultProjectTo<PostCategoryDetailModel>().FirstOrDefaultAsync();

            if (detailModel == null)
                throw AppValidationException.From(resultLocalizer, ResultCode.EntityNotFound);

            var locQuery = dbContext.PostCategoryLocalization.ByEntity(model.Id)
                .Select(o => new PostCategoryLocalizationEntity
                {
                    Description = o.Description,
                    Lang = o.Lang,
                    Region = o.Region,
                    Title = o.Title
                });

            string currentLang = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            string lang = model.Lang;
            string region = model.Region;
            if (string.IsNullOrEmpty(lang))
                lang = currentLang;

            var locEntity = await locQuery.ByCulture(lang, region).FirstOrDefaultAsync();

            if (locEntity == null && model.Fallback)
            {
                if (lang != currentLang)
                    locEntity = await locQuery.ByCulture(currentLang).FirstOrDefaultAsync();

                if (locEntity == null)
                    locEntity = await locQuery.FirstOrDefaultAsync();
            }

            if (locEntity != null)
                locEntity.CopyTo(detailModel);

            return detailModel;
        }

        public async Task<int> CreatePostCategoryAsync(CreatePostCategoryModel model)
        {
            #region Validation
            var userInfo = contextProvider.BusinessContext.PrincipalInfo;
            var validationData = new ValidationData(resultLocalizer);

            // validation logic here

            if (!validationData.IsValid)
                throw validationData.BuildException();
            #endregion

            var entity = model.MapTo<PostCategoryEntity>();
            PrepareCreate(entity);

            entity = dbContext.PostCategory.Add(entity).Entity;

            await dbContext.SaveChangesAsync();

            return entity.Id;
        }

        public async Task UpdatePostCategoryAsync(int id, UpdatePostCategoryModel model)
        {
            #region Validation
            var userInfo = contextProvider.BusinessContext.PrincipalInfo;
            var validationData = new ValidationData(resultLocalizer);

            var entity = await dbContext.PostCategory.ById(id).Select(o => new PostCategoryEntity
            {
                Id = o.Id
            }).FirstOrDefaultAsync();

            if (entity == null)
                validationData.Fail(code: ResultCode.EntityNotFound);

            if (!validationData.IsValid)
                throw validationData.BuildException();
            #endregion

            model.CopyTo(entity);
            PrepareUpdate(entity);

            entity = dbContext.Update(entity, o => o.StartingPostId).Entity;

            await dbContext.SaveChangesAsync();
        }

        public async Task DeletePostCategoryAsync(int id)
        {
            #region Validation
            var userInfo = contextProvider.BusinessContext.PrincipalInfo;
            var validationData = new ValidationData(resultLocalizer);

            var entity = await dbContext.PostCategory.ById(id).Select(o => new PostCategoryEntity
            {
                Id = o.Id,
            }).FirstOrDefaultAsync();

            if (entity == null)
                validationData.Fail(code: ResultCode.EntityNotFound);

            if (!validationData.IsValid)
                throw validationData.BuildException();
            #endregion

            entity = dbContext.Remove(entity).Entity;

            await dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<int>> AddPostCategoryLocalizationsAsync(int postCategoryId, AddPostCategoryLocalizationsModel model)
        {
            #region Validation
            var userInfo = contextProvider.BusinessContext.PrincipalInfo;
            var validationData = new ValidationData(resultLocalizer);

            var exists = await dbContext.PostCategory.ById(postCategoryId).AnyAsync();

            if (!exists)
                validationData.Fail(code: ResultCode.EntityNotFound);

            var cultures = model.ListOfLocalization.Select(o => I18nHelper.GetCulture(o.Lang, o.Region)).ToArray();
            var anyCultureExists = await dbContext.PostCategoryLocalization.ByCultures(cultures).AnyAsync();
            if (anyCultureExists)
                validationData.Fail(code: ResultCode.PostCategory_LocalizationExists);

            if (!validationData.IsValid)
                throw validationData.BuildException();
            #endregion

            var entities = model.ListOfLocalization.MapTo<PostCategoryLocalizationEntity>().ToArray();
            foreach (var entity in entities) entity.EntityId = postCategoryId;

            dbContext.AddRange(entities);

            await dbContext.SaveChangesAsync();

            return entities.Select(o => o.Id).ToArray();
        }

        public async Task UpdatePostCategoryLocalizationsAsync(int postCategoryId, UpdatePostCategoryLocalizationsModel model)
        {
            #region Validation
            var userInfo = contextProvider.BusinessContext.PrincipalInfo;
            var validationData = new ValidationData(resultLocalizer);

            var updatedLocalizationIds = model.ListOfLocalization.Select(o => o.Id).ToArray();
            var entities = await dbContext.PostCategoryLocalization.ByIds(updatedLocalizationIds)
                .Select(o => new PostCategoryLocalizationEntity
                {
                    Id = o.Id,
                    EntityId = o.EntityId
                }).ToArrayAsync();

            if (entities.Any(o => o.EntityId != postCategoryId)
                || entities.Length != updatedLocalizationIds.Length)
                validationData.Fail(code: ResultCode.PostCategory_InvalidUpdateLocalizationsRequest);

            if (!validationData.IsValid)
                throw validationData.BuildException();
            #endregion

            foreach (var entity in entities)
            {
                model.ListOfLocalization.First(o => o.Id == entity.Id).CopyTo(entity);

                dbContext.Update(entity, o => o.Description,
                    o => o.Title);
            }

            await dbContext.SaveChangesAsync();
        }

        public async Task DeletePostCategoryLocalizationsAsync(int postCategoryId, IEnumerable<int> localizationIds)
        {
            #region Validation
            var userInfo = contextProvider.BusinessContext.PrincipalInfo;
            var validationData = new ValidationData(resultLocalizer);

            var entities = await dbContext.PostCategoryLocalization.ByIds(localizationIds)
                .Select(o => new PostCategoryLocalizationEntity
                {
                    Id = o.Id,
                    EntityId = o.EntityId
                }).ToArrayAsync();

            if (entities.Any(o => o.EntityId != postCategoryId)
                || entities.Length != localizationIds.Count())
                validationData.Fail(code: ResultCode.PostCategory_InvalidDeleteLocalizationsRequest);

            if (!validationData.IsValid)
                throw validationData.BuildException();
            #endregion

            dbContext.RemoveRange(entities);

            await dbContext.SaveChangesAsync();
        }

        private void PrepareCreate(PostCategoryEntity entity)
        {
        }

        private void PrepareUpdate(PostCategoryEntity entity)
        {
        }
    }
}
