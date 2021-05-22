using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TFW.Framework.CQRSExamples.Entities.Relational;
using TFW.Framework.CQRSExamples.Models.Query;

namespace TFW.Framework.CQRSExamples.Queries
{
    public class ProductQuery : IProductQuery
    {
        private readonly RelationalContext _relationalContext;

        public ProductQuery(RelationalContext relationalContext)
        {
            _relationalContext = relationalContext;
        }

        public async Task<ProductDetail> GetProductDetailAsync(string id)
        {
            var detail = await _relationalContext.Products
                .Select(o => new ProductDetail
                {
                    Id = o.Id,
                    Description = o.Description,
                    Name = o.Name,
                    CategoryId = o.CategoryId,
                    CategoryName = o.Category.Name,
                    UnitPrice = o.UnitPrice
                }).FirstOrDefaultAsync(o => o.Id == id);

            return detail;
        }

        public async Task<IEnumerable<ProductListItem>> GetProductListAsync()
        {
            var list = await _relationalContext.Products.Select(o => new ProductListItem
            {
                Id = o.Id,
                Name = o.Name,
                CategoryId = o.CategoryId,
                CategoryName = o.Category.Name,
                UnitPrice = o.UnitPrice
            }).ToArrayAsync();

            return list;
        }
    }
}
