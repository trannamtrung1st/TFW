using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TFW.Framework.CQRSExamples.Models.Command;
using TFW.Framework.CQRSExamples.Models.Query;

namespace TFW.Framework.CQRSExamples.Pages.Product
{
    public class CreateModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly IProductCategoryQuery _productCategoryQuery;
        private readonly IStoreQuery _storeQuery;
        private readonly IBrandQuery _brandQuery;

        public CreateModel(IMediator mediator,
            IProductCategoryQuery productCategoryQuery,
            IStoreQuery storeQuery,
            IBrandQuery brandQuery)
        {
            _mediator = mediator;
            _productCategoryQuery = productCategoryQuery;
            _storeQuery = storeQuery;
            _brandQuery = brandQuery;
        }

        [BindProperty]
        public CreateProductCommand Command { get; set; }

        public IEnumerable<SelectListItem> ProductCategories { get; set; }
        public IEnumerable<SelectListItem> Stores { get; set; }
        public IEnumerable<SelectListItem> Brands { get; set; }

        public async Task OnGet()
        {
            ProductCategories = (await _productCategoryQuery.GetProductCategoryListOptionAsync())
                .Select(o => new SelectListItem
                {
                    Text = o.Name,
                    Value = o.Id
                }).ToArray();

            Stores = (await _storeQuery.GetStoreListOptionAsync())
                .Select(o => new SelectListItem
                {
                    Text = o.StoreName,
                    Value = o.Id
                }).ToArray();

            Brands = (await _brandQuery.GetBrandListOptionAsync())
                .Select(o => new SelectListItem
                {
                    Text = o.Name,
                    Value = o.Id
                }).ToArray();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var id = await _mediator.Send(Command);

            if (id == null) throw new Exception("Failed to create product");

            return RedirectToPage("/Product/Index");
        }
    }
}
