using Application.Employees.Queries.GetEmployeesList;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Employees
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IGetEmployeesListQuery _query;

        public EmployeesController(IGetEmployeesListQuery query)
        {
            _query = query;
        }

        [HttpGet("")]
        public async Task<IEnumerable<EmployeeModel>> GetEmployeesListAsync()
        {
            return await _query.ExecuteAsync();
        }
    }
}
