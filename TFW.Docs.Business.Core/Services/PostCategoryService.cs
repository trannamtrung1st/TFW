using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFW.Docs.Business.Core.Queries;
using TFW.Docs.Business.Services;
using TFW.Docs.Cross;
using TFW.Docs.Cross.Entities;
using TFW.Docs.Cross.Models.Common;
using TFW.Docs.Cross.Models.PostCategory;
using TFW.Docs.Cross.Providers;
using TFW.Docs.Data;
using TFW.Framework.AutoMapper;
using TFW.Framework.DI.Attributes;

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

        public async Task<IEnumerable<int>> AddPostCategoryLocalizationsAsync(int postCategoryId, AddPostCategoryLocalizationsModel model)
        {
            #region Validation
            var userInfo = contextProvider.BusinessContext.PrincipalInfo;
            var validationData = new ValidationData(resultLocalizer);

            var exists = await dbContext.PostCategory.ById(postCategoryId).AnyAsync();

            if (!exists)
                validationData.Fail(code: ResultCode.EntityNotFound);

            var cultures = model.ListOfLocalization.Select(o => (string.IsNullOrEmpty(o.Region) ? o.Lang : (o.Lang + "-" + o.Region))).ToArray();
            var anyCultureExists = await dbContext.PostCategoryLocalization.ByCultures(cultures).AnyAsync();
            if (anyCultureExists)
                validationData.Fail(code: ResultCode.PostCategory_InvalidPostCategoryLocalizationExists);

            if (!validationData.IsValid)
                throw validationData.BuildException();
            #endregion

            var entities = model.ListOfLocalization.MapTo<PostCategoryLocalizationEntity>().ToArray();
            foreach (var entity in entities) entity.EntityId = postCategoryId;

            dbContext.AddRange(entities);

            await dbContext.SaveChangesAsync();

            return entities.Select(o => o.Id).ToArray();
        }

        private void PrepareCreate(PostCategoryEntity entity)
        {
        }

        private void PrepareUpdate(PostCategoryEntity entity)
        {
        }
    }
}
