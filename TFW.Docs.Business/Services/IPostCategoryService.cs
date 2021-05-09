using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using TFW.Docs.Cross.Models.Common;
using TFW.Docs.Cross.Models.PostCategory;

namespace TFW.Docs.Business.Services
{
    public interface IPostCategoryService
    {
        Task<ListResponseModel<TModel>> GetListPostCategoryAsync<TModel>(
            ListPostCategoryRequestModel requestModel, ParsingConfig parsingConfig = null);
        Task<int> CreatePostCategoryAsync(CreatePostCategoryModel model);
        Task UpdatePostCategoryAsync(int id, UpdatePostCategoryModel model);
        Task DeletePostCategoryAsync(int id);
        Task<IEnumerable<int>> AddPostCategoryLocalizationsAsync(int postCategoryId, AddPostCategoryLocalizationsModel model);
        Task UpdatePostCategoryLocalizationsAsync(int postCategoryId, UpdatePostCategoryLocalizationsModel model);
        Task DeletePostCategoryLocalizationsAsync(int postCategoryId, IEnumerable<int> localizationIds);
    }
}
