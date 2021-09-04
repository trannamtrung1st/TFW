using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using TFW.Framework.CQRSExamples.Models.Command;
using TFW.Framework.CQRSExamples.Models.Query;

namespace TFW.Framework.CQRSExamples.Pages.ProductCategory
{
    public class UpdateModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly IProductCategoryQuery _productCategoryQuery;

        public UpdateModel(IMediator mediator,
            IProductCategoryQuery productCategoryQuery)
        {
            _mediator = mediator;
            _productCategoryQuery = productCategoryQuery;
        }

        [FromRoute]
        public string Id { get; set; }

        [BindProperty]
        public UpdateProductCategoryCommand Command { get; set; }

        public string Message { get; set; }

        public async Task OnGetAsync()
        {
            var detail = await _productCategoryQuery.GetProductCategoryDetailAsync(Id);
            Command = new UpdateProductCategoryCommand
            {
                Id = detail.Id,
                Description = detail.Description,
                Name = detail.Name
            };
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Command.Id = Id;

            await _mediator.Send(Command);

            Message = "Update successfully";

            return Page();
        }
    }
}
