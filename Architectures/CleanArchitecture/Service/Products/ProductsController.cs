using Application.Products.Queries.GetProductsList;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Products
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IGetProductsListQuery _query;

        public ProductsController(IGetProductsListQuery query)
        {
            _query = query;
        }

        [HttpGet("")]
        public async Task<IEnumerable<ProductModel>> GetProductsListAsync()
        {
            return await _query.ExecuteAsync();
        }
    }
}
