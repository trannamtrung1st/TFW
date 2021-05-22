using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
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
        private readonly IMemoryCache _cache;

        public ProductQuery(RelationalContext relationalContext,
            IMemoryCache cache)
        {
            _relationalContext = relationalContext;
            _cache = cache;
        }

        public async Task<ProductDetail> GetProductDetailAsync(string id)
        {
            var detail = (await GetProductEntities()).Select(o => new ProductDetail
            {
                Id = o.Id,
                Description = o.Description,
                Name = o.Name,
                CategoryId = o.CategoryId,
                CategoryName = o.Category?.Name,
                StoreId = o.StoreId,
                StoreName = o.Store?.StoreName,
                BrandId = o.BrandId,
                BrandName = o.Brand?.Name,
                UnitPrice = o.UnitPrice
            }).FirstOrDefault(o => o.Id == id);

            return detail;
        }

        public async Task<IEnumerable<ProductListItem>> GetProductListAsync()
        {
            var list = (await GetProductEntities()).Select(o => new ProductListItem
            {
                Id = o.Id,
                Name = o.Name,
                CategoryId = o.CategoryId,
                CategoryName = o.Category?.Name,
                BrandName = o.Brand?.Name,
                StoreName = o.Store?.StoreName,
                UnitPrice = o.UnitPrice
            }).ToArray();

            return list;
        }

        private async Task<IEnumerable<ProductEntity>> GetProductEntities()
        {
            var products = await _cache.GetOrCreateAsync<List<ProductEntity>>(nameof(ProductEntity),
                async entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
                    return await _relationalContext.Products
                        .Select(o => new ProductEntity
                        {
                            Id = o.Id,
                            Store = o.Store,
                            Name = o.Name,
                            Brand = o.Brand,
                            BrandId = o.BrandId,
                            Category = o.Category,
                            CategoryId = o.CategoryId,
                            Description = o.Description,
                            StoreId = o.StoreId,
                            UnitPrice = o.UnitPrice
                        }).ToListAsync();
                });

            return products;
        }
    }
}
