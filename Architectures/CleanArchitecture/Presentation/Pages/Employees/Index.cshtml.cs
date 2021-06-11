using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Employees.Queries.GetEmployeesList;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Pages.Employees
{
    public class IndexModel : PageModel
    {
        private readonly IGetEmployeesListQuery _query;

        public IndexModel(IGetEmployeesListQuery query)
        {
            _query = query;
        }

        public IEnumerable<EmployeeModel> Employees { get; set; }

        public async Task OnGetAsync()
        {
            Employees = await _query.ExecuteAsync();
        }
    }
}
