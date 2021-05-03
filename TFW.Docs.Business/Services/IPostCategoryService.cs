using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TFW.Docs.Cross.Models.PostCategory;

namespace TFW.Docs.Business.Services
{
    public interface IPostCategoryService
    {
        Task<int> CreatePostCategoryAsync(CreatePostCategoryModel model);
        Task UpdatePostCategoryAsync(int id, UpdatePostCategoryModel model);
    }
}
