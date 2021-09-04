using MediatR;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TFW.Framework.CQRSExamples.Entities.Relational;

namespace TFW.Framework.CQRSExamples.Models.Notification
{
    public class DeleteProductEvent : INotification
    {
        public string Id { get; set; }
    }

    public class DeleteProductEventHandler : INotificationHandler<DeleteProductEvent>
    {
        private readonly IMemoryCache _cache;
        private readonly RelationalContext _relationalContext;

        public DeleteProductEventHandler(IMemoryCache cache,
            RelationalContext relationalContext)
        {
            _cache = cache;
            _relationalContext = relationalContext;
        }

        public Task Handle(DeleteProductEvent notification, CancellationToken cancellationToken)
        {
            var cachedProducts = _cache.Get<List<ProductEntity>>(nameof(ProductEntity));

            if (cachedProducts == null) return Task.CompletedTask;

            var removedItem = cachedProducts.Find(o => o.Id == notification.Id);

            if (removedItem == null) return Task.CompletedTask;

            cachedProducts.Remove(removedItem);

            return Task.CompletedTask;
        }
    }
}
