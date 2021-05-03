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
            IStringLocalizer<ResultCodeResources> resultLocalizer,
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

            var entity = dbContext.PostCategory.ById(id).Select(o => new PostCategoryEntity
            {
                Id = o.Id
            }).FirstOrDefault();

            if (entity == null)
                validationData.Fail(code: ResultCode.EntityNotFound);

            if (!validationData.IsValid)
                throw validationData.BuildException();
            #endregion

            model.CopyTo(entity);
            PrepareUpdate(entity);

            entity = dbContext.Update(entity, o => o.StartingPostId).Entity;

            foreach (var updateModel in model.ListOfUpdatedLocalization)
            {
                var localization = updateModel.MapTo<PostCategoryLocalizationEntity>();

                dbContext.Update(localization, o => o.Title,
                    o => o.Description);
            }

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
