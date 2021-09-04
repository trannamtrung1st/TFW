using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using TFW.Framework.DI.WebExamples.Builders;
using TFW.Framework.DI.WebExamples.Models;
using TFW.Framework.DI.WebExamples.Repositories;

namespace TFW.Framework.DI.WebExamples.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IProductRepository _productRepository;
        private readonly IAppSettings _appSettings;

        public ProductController(DataContext dataContext,
            IProductRepository productRepository,
            IAppSettings appSettings)
        {
            _dataContext = dataContext;
            _productRepository = productRepository;
            _appSettings = appSettings;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productRepository.Get().Select(o => new
            {
                o.Id,
                o.Name
            }).ToArrayAsync();

            return Ok(products);
        }

        [HttpPost("")]
        public async Task<IActionResult> Create(Product product)
        {
            product = _productRepository.Create(product);

            await _dataContext.SaveChangesAsync();

            return Ok(new
            {
                product.Id,
                product.Name
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Product product)
        {
            product.Id = id;
            product = _productRepository.Update(product);
            await _dataContext.SaveChangesAsync();
            return Ok(new
            {
                product.Id,
                product.Name
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _productRepository.Delete(id);
            await _dataContext.SaveChangesAsync();
            return Ok(id);
        }

        [HttpPost("build")]
        public async Task<IActionResult> BuildProduct(int amount)
        {
            var provider = HttpContext.RequestServices;
            var random = new Random();
            for (var i = 0; i < amount; i++)
            {
                var builder = provider.GetRequiredService<IProductBuilder>();
                var product = builder.Id(random.Next())
                    .Name(Guid.NewGuid().ToString())
                    .Build();
                _dataContext.Add(product);
            }

            var success = await _dataContext.SaveChangesAsync();
            return Ok(success);
        }
    }
}
