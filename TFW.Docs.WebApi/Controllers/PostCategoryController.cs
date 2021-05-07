using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using TFW.Docs.Business;
using TFW.Docs.Business.Services;
using TFW.Docs.Cross;
using TFW.Docs.Cross.Models.Common;
using TFW.Docs.Cross.Models.PostCategory;
using TFW.Docs.Cross.Providers;
using TFW.Framework.Web.Attributes;

namespace TFW.Docs.WebApi.Controllers
{
    [Route(Routing.Controller.PostCategory.Route)]
    [Authorize]
    public class PostCategoryController : BaseApiController
    {
        private readonly IPostCategoryService _postCategoryService;

        public PostCategoryController(IUnitOfWork unitOfWork,
            IBusinessContextProvider contextProvider,
            IStringLocalizer<ResultCode> resultLocalizer,
            IPostCategoryService postCategoryService) : base(unitOfWork, contextProvider, resultLocalizer)
        {
            _postCategoryService = postCategoryService;
        }

        [SwaggerResponse((int)HttpStatusCode.OK, null, typeof(AppResult<int>))]
        [HttpPost(Routing.Controller.PostCategory.CreatePostCategory)]
        public async Task<IActionResult> CreatePostCategory(CreatePostCategoryModel model)
        {
            var id = await _postCategoryService.CreatePostCategoryAsync(model);

            return Success(id);
        }

        // [TODO] Add validation
        [SwaggerResponse((int)HttpStatusCode.NoContent)]
        [HttpPatch(Routing.Controller.PostCategory.UpdatePostCategory)]
        public async Task<IActionResult> UpdatePostCategory(int id, UpdatePostCategoryModel model)
        {
            await _postCategoryService.UpdatePostCategoryAsync(id, model);

            return NoContent();
        }

        [SwaggerResponse((int)HttpStatusCode.OK, null, typeof(AppResult<IEnumerable<int>>))]
        [HttpPost(Routing.Controller.PostCategory.AddLocalizations)]
        public async Task<IActionResult> AddLocalizations(int id, AddPostCategoryLocalizationsModel model)
        {
            var ids = await _postCategoryService.AddPostCategoryLocalizationsAsync(id, model);

            return Success(ids);
        }
    }
}
