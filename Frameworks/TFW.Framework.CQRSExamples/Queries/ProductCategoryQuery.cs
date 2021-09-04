using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TFW.Framework.CQRSExamples.Entities.Relational;
using TFW.Framework.CQRSExamples.Models.Query;

namespace TFW.Framework.CQRSExamples.Queries
{
    public class ProductCategoryQuery : IProductCategoryQuery
    {
        private readonly RelationalContext _relationalContext;

        public ProductCategoryQuery(RelationalContext relationalContext)
        {
            _relationalContext = relationalContext;
        }

        public async Task<ProductCategoryDetail> GetProductCategoryDetailAsync(string id)
        {
            var detail = await _relationalContext.ProductCategories
                .Select(o => new ProductCategoryDetail
                {
                    Id = o.Id,
                    Description = o.Description,
                    Name = o.Name
                }).FirstOrDefaultAsync(o => o.Id == id);

            return detail;
        }

        public async Task<IEnumerable<ProductCategoryListItem>> GetProductCategoryListAsync()
        {
            var list = await _relationalContext.ProductCategories.Select(o => new ProductCategoryListItem
            {
                Description = o.Description,
                Id = o.Id,
                Name = o.Name,
                ProductCount = o.Products.Count()
            }).ToArrayAsync();

            return list;
        }

        public async Task<IEnumerable<ProductCategoryListOption>> GetProductCategoryListOptionAsync()
        {
            var list = await _relationalContext.ProductCategories.Select(o => new ProductCategoryListOption
            {
                Id = o.Id,
                Name = o.Name,
            }).ToArrayAsync();

            return list;
        }
    }
}
