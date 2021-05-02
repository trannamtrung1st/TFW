using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TFW.Docs.Business.Services;
using TFW.Docs.Cross;
using TFW.Docs.Cross.Models.PostCategory;
using TFW.Docs.Cross.Providers;
using TFW.Docs.Data;
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

        public Task<int> CreatePostCategory(CreatePostCategoryModel model)
        {
            throw new NotImplementedException();
        }
    }
}
