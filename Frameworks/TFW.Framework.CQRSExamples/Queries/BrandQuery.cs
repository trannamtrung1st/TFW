using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TFW.Framework.CQRSExamples.Entities.Relational;
using TFW.Framework.CQRSExamples.Models.Query;

namespace TFW.Framework.CQRSExamples.Queries
{
    public class BrandQuery : IBrandQuery
    {
        private readonly IMemoryCache _cache;

        public BrandQuery(IMemoryCache cache)
        {
            _cache = cache;
        }

        public Task<IEnumerable<BrandListItem>> GetBrandListAsync()
        {
            var stores = _cache.Get<IEnumerable<BrandEntity>>(nameof(BrandEntity))
                .Select(o => new BrandListItem
                {
                    Id = o.Id,
                    Name = o.Name,
                    Description = o.Description
                });

            return Task.FromResult(stores);
        }

        public Task<IEnumerable<BrandListOption>> GetBrandListOptionAsync()
        {
            var stores = _cache.Get<IEnumerable<BrandEntity>>(nameof(BrandEntity))
                .Select(o => new BrandListOption
                {
                    Id = o.Id,
                    Name = o.Name
                });

            return Task.FromResult(stores);
        }
    }
}
