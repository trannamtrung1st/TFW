using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TFW.Framework.CQRSExamples.Entities.Relational;
using TFW.Framework.CQRSExamples.Models.Query;

namespace TFW.Framework.CQRSExamples.Queries
{
    public class StoreQuery : IStoreQuery
    {
        private readonly IMemoryCache _cache;

        public StoreQuery(IMemoryCache cache)
        {
            _cache = cache;
        }

        public Task<IEnumerable<StoreListItem>> GetStoreListAsync()
        {
            var stores = _cache.Get<IEnumerable<StoreEntity>>(nameof(StoreEntity))
                .Select(o => new StoreListItem
                {
                    Address = o.Address,
                    Id = o.Id,
                    StoreName = o.StoreName
                });

            return Task.FromResult(stores);
        }

        public Task<IEnumerable<StoreListOption>> GetStoreListOptionAsync()
        {
            var stores = _cache.Get<IEnumerable<StoreEntity>>(nameof(StoreEntity))
                .Select(o => new StoreListOption
                {
                    Id = o.Id,
                    StoreName = o.StoreName
                });

            return Task.FromResult(stores);
        }
    }
}
