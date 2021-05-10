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

        [SwaggerResponse((int)HttpStatusCode.OK, null, typeof(AppResult<ListResponseModel<ListPostCategoryModel>>))]
        [HttpGet(Routing.Controller.PostCategory.GetListPostCategory)]
        public async Task<IActionResult> GetListPostCategory([FromQuery] ListPostCategoryRequestModel model)
        {
            var data = await _postCategoryService.GetListPostCategoryAsync<ListPostCategoryModel>(model);

            return Success(data);
        }

        [SwaggerResponse((int)HttpStatusCode.OK, null, typeof(AppResult<PostCategoryDetailModel>))]
        [HttpGet(Routing.Controller.PostCategory.GetPostCategoryDetail)]
        public async Task<IActionResult> GetPostCategoryDetail([FromQuery] PostCategoryDetailRequestModel model)
        {
            var data = await _postCategoryService.GetPostCategoryDetailAsync(model);

            return Success(data);
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

        [SwaggerResponse((int)HttpStatusCode.NoContent)]
        [HttpDelete(Routing.Controller.PostCategory.DeletePostCategory)]
        public async Task<IActionResult> DeletePostCategory(int id)
        {
            await _postCategoryService.DeletePostCategoryAsync(id);

            return NoContent();
        }

        [SwaggerResponse((int)HttpStatusCode.OK, null, typeof(AppResult<IEnumerable<int>>))]
        [HttpPost(Routing.Controller.PostCategory.AddLocalizations)]
        public async Task<IActionResult> AddLocalizations(int id, AddPostCategoryLocalizationsModel model)
        {
            var ids = await _postCategoryService.AddPostCategoryLocalizationsAsync(id, model);

            return Success(ids);
        }

        [SwaggerResponse((int)HttpStatusCode.NoContent)]
        [HttpPut(Routing.Controller.PostCategory.UpdateLocalizations)]
        public async Task<IActionResult> UpdateLocalizations(int id, UpdatePostCategoryLocalizationsModel model)
        {
            await _postCategoryService.UpdatePostCategoryLocalizationsAsync(id, model);

            return NoContent();
        }

        [SwaggerResponse((int)HttpStatusCode.NoContent)]
        [HttpDelete(Routing.Controller.PostCategory.DeleteLocalizations)]
        public async Task<IActionResult> DeleteLocalizations(int id, [FromBody] IEnumerable<int> localizationIds)
        {
            await _postCategoryService.DeletePostCategoryLocalizationsAsync(id, localizationIds);

            return NoContent();
        }
    }
}
