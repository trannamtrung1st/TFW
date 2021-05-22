using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TFW.Framework.CQRSExamples.Models.Command;
using TFW.Framework.CQRSExamples.Models.Query;

namespace TFW.Framework.CQRSExamples.Pages.Product
{
    public class UpdateModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly IProductCategoryQuery _productCategoryQuery;
        private readonly IProductQuery _productQuery;
        private readonly IStoreQuery _storeQuery;
        private readonly IBrandQuery _brandQuery;

        public UpdateModel(IMediator mediator,
            IProductCategoryQuery productCategoryQuery,
            IProductQuery productQuery,
            IStoreQuery storeQuery,
            IBrandQuery brandQuery)
        {
            _mediator = mediator;
            _productCategoryQuery = productCategoryQuery;
            _productQuery = productQuery;
            _storeQuery = storeQuery;
            _brandQuery = brandQuery;
        }

        [FromRoute]
        public string Id { get; set; }

        [BindProperty]
        public UpdateProductCommand Command { get; set; }

        public IEnumerable<SelectListItem> ProductCategories { get; set; }
        public IEnumerable<SelectListItem> Stores { get; set; }
        public IEnumerable<SelectListItem> Brands { get; set; }

        public string Message { get; set; }

        public async Task OnGetAsync()
        {
            var detail = await _productQuery.GetProductDetailAsync(Id);

            Command = new UpdateProductCommand
            {
                Id = detail.Id,
                Description = detail.Description,
                Name = detail.Name,
                CategoryId = detail.CategoryId,
                BrandId = detail.BrandId,
                StoreId = detail.StoreId,
                UnitPrice = detail.UnitPrice
            };

            await LoadData();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Command.Id = Id;

            await _mediator.Send(Command);

            Message = "Update successfully";

            await LoadData();

            return Page();
        }

        private async Task LoadData()
        {
            ProductCategories = (await _productCategoryQuery.GetProductCategoryListOptionAsync())
                .Select(o => new SelectListItem
                {
                    Text = o.Name,
                    Value = o.Id,
                    Selected = o.Id == Command.CategoryId
                }).ToArray();

            Stores = (await _storeQuery.GetStoreListOptionAsync())
                .Select(o => new SelectListItem
                {
                    Text = o.StoreName,
                    Value = o.Id,
                    Selected = o.Id == Command.StoreId
                }).ToArray();

            Brands = (await _brandQuery.GetBrandListOptionAsync())
                .Select(o => new SelectListItem
                {
                    Text = o.Name,
                    Value = o.Id,
                    Selected = o.Id == Command.BrandId
                }).ToArray();
        }
    }
}
