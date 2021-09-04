using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TFW.Framework.CQRSExamples.Entities.Relational;

namespace TFW.Framework.CQRSExamples.Models.Notification
{
    public class UpdateProductEvent : INotification
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double UnitPrice { get; set; }
        public string CategoryId { get; set; }
        public string StoreId { get; set; }
        public string BrandId { get; set; }
    }

    public class UpdateProductEventHandler : INotificationHandler<UpdateProductEvent>
    {
        private readonly IMemoryCache _cache;
        private readonly RelationalContext _relationalContext;

        public UpdateProductEventHandler(IMemoryCache cache,
            RelationalContext relationalContext)
        {
            _cache = cache;
            _relationalContext = relationalContext;
        }

        public async Task Handle(UpdateProductEvent notification, CancellationToken cancellationToken)
        {
            var cachedProducts = _cache.Get<List<ProductEntity>>(nameof(ProductEntity));

            if (cachedProducts == null) return;

            var idx = cachedProducts.FindIndex(o => o.Id == notification.Id);

            if (idx < 0) return;

            var product = await _relationalContext.Products.Select(o => new ProductEntity
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
            }).FirstOrDefaultAsync(o => o.Id == notification.Id);

            cachedProducts[idx] = product;
        }
    }
}
