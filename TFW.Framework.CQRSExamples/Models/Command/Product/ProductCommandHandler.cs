using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TFW.Framework.CQRSExamples.Entities.Relational;
using TFW.Framework.CQRSExamples.Models.Notification;

namespace TFW.Framework.CQRSExamples.Models.Command
{
    public class ProductCommandHandler : IRequestHandler<CreateProductCommand, string>,
        IRequestHandler<UpdateProductCommand>,
        IRequestHandler<DeleteProductCommand>
    {
        private readonly RelationalContext _relationalContext;
        private readonly IMediator _mediator;

        public ProductCommandHandler(
            RelationalContext relationalContext,
            IMediator mediator)
        {
            _relationalContext = relationalContext;
            _mediator = mediator;
        }

        public async Task<string> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var entity = new ProductEntity
            {
                Id = Guid.NewGuid().ToString(),
                Description = request.Description,
                Name = request.Name,
                CategoryId = request.CategoryId,
                StoreId = request.StoreId,
                BrandId = request.BrandId,
                UnitPrice = request.UnitPrice
            };

            _relationalContext.Add(entity);

            await _relationalContext.SaveChangesAsync();

            await _mediator.Publish(new CreateProductEvent
            {
                BrandId = entity.BrandId,
                CategoryId = entity.CategoryId,
                Description = entity.Description,
                Id = entity.Id,
                Name = entity.Name,
                StoreId = entity.StoreId,
                UnitPrice = entity.UnitPrice
            });

            return entity.Id;
        }

        public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var entity = new ProductEntity
            {
                Id = request.Id,
                Description = request.Description,
                Name = request.Name,
                CategoryId = request.CategoryId,
                StoreId = request.StoreId,
                BrandId = request.BrandId,
                UnitPrice = request.UnitPrice
            };

            _relationalContext.Update(entity);

            await _relationalContext.SaveChangesAsync();

            await _mediator.Publish(new UpdateProductEvent
            {
                BrandId = entity.BrandId,
                CategoryId = entity.CategoryId,
                Description = entity.Description,
                Id = entity.Id,
                Name = entity.Name,
                StoreId = entity.StoreId,
                UnitPrice = entity.UnitPrice
            });

            return Unit.Value;
        }

        public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            _relationalContext.Products.Remove(new ProductEntity { Id = request.Id });

            await _relationalContext.SaveChangesAsync();

            await _mediator.Publish(new DeleteProductEvent
            {
                Id = request.Id
            });

            return Unit.Value;
        }
    }
}
