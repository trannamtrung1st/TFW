using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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

        public async Task<int> CreatePostCategory(CreatePostCategoryModel model)
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

        private void PrepareCreate(PostCategoryEntity entity)
        {
        }
    }
}
