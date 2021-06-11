using Application.Customers.Queries.GetCustomersList;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Customers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly IGetCustomersListQuery _query;

        public CustomersController(IGetCustomersListQuery query)
        {
            _query = query;
        }

        [HttpGet("")]
        public async Task<IEnumerable<CustomerModel>> GetCustomersListAsync()
        {
            return await _query.ExecuteAsync();
        }
    }
}
