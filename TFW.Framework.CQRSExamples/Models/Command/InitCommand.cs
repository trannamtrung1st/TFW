using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TFW.Framework.CQRSExamples.Entities.Relational;

namespace TFW.Framework.CQRSExamples.Models.Command
{
    public class InitCommand : IRequest
    {
    }

    public class InitCommandHandler : IRequestHandler<InitCommand>
    {
        private readonly IMemoryCache _cache;
        private readonly RelationalContext _relationalContext;

        public InitCommandHandler(IMemoryCache cache,
            RelationalContext relationalContext)
        {
            _cache = cache;
            _relationalContext = relationalContext;
        }

        public async Task<Unit> Handle(InitCommand request, CancellationToken cancellationToken)
        {
            var isInit = _relationalContext.Brands.Any();

            if (!isInit)
            {
                var range = Enumerable.Range(1, 10);

                var brandEntities = range.Select(o => new BrandEntity
                {
                    Id = o.ToString(),
                    Description = $"Brand {o} Desc",
                    Name = $"Brand {o}"
                }).ToArray();

                var storeEntities = range.Select(o => new StoreEntity
                {
                    Id = o.ToString(),
                    StoreName = $"Store {o}",
                    Address = Guid.NewGuid().ToString()
                }).ToArray();

                _relationalContext.Brands.AddRange(brandEntities);
                _relationalContext.Stores.AddRange(storeEntities);

                await _relationalContext.SaveChangesAsync();
            }

            var brands = await _relationalContext.Brands.ToArrayAsync();
            var stores = await _relationalContext.Stores.ToArrayAsync();

            _cache.Set(nameof(BrandEntity), brands);
            _cache.Set(nameof(StoreEntity), stores);

            return Unit.Value;
        }
    }
}
