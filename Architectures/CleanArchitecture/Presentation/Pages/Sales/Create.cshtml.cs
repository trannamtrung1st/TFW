using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Customers.Queries.GetCustomersList;
using Application.Employees.Queries.GetEmployeesList;
using Application.Products.Queries.GetProductsList;
using Application.Sales.Commands.CreateSale;
using Domain.Sales;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Presentation.Pages.Sales
{
    public class CreateModel : PageModel
    {
        private readonly IGetCustomersListQuery _customersQuery;
        private readonly IGetEmployeesListQuery _employeesQuery;
        private readonly IGetProductsListQuery _productsQuery;
        private readonly IMediator _mediator;

        public CreateModel(
            IGetCustomersListQuery customersQuery,
            IGetEmployeesListQuery employeesQuery,
            IGetProductsListQuery productsQuery,
            IMediator mediator)
        {
            _customersQuery = customersQuery;
            _employeesQuery = employeesQuery;
            _productsQuery = productsQuery;
            _mediator = mediator;
        }

        public IEnumerable<SelectListItem> Customers { get; set; }

        public IEnumerable<SelectListItem> Employees { get; set; }

        public IEnumerable<SelectListItem> Products { get; set; }

        [BindProperty]
        public CreateSaleCommand Sale { get; set; }

        public async Task OnGetAsync()
        {
            await InitAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            _ = await _mediator.Send(Sale);

            return RedirectToPage("/Sales/Index");
        }

        public async Task InitAsync()
        {
            var employees = await _employeesQuery.ExecuteAsync();

            var customers = await _customersQuery.ExecuteAsync();

            var products = await _productsQuery.ExecuteAsync();

            Employees = employees
                .Select(p => new SelectListItem()
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                }).ToArray();

            Customers = customers
                .Select(p => new SelectListItem()
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                }).ToArray();

            Products = products
                .Select(p => new SelectListItem()
                {
                    Value = p.Id.ToString(),
                    Text = p.Name + " ($" + p.UnitPrice + ")"
                }).ToArray();

            Sale = new CreateSaleCommand();
        }
    }
}
