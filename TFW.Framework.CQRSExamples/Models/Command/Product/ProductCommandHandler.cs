using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TFW.Framework.CQRSExamples.Entities.Relational;

namespace TFW.Framework.CQRSExamples.Models.Command
{
    public class ProductCommandHandler : IRequestHandler<CreateProductCommand, string>,
        IRequestHandler<UpdateProductCommand>,
        IRequestHandler<DeleteProductCommand>
    {
        private readonly RelationalContext _relationalContext;

        public ProductCommandHandler(
            RelationalContext relationalContext)
        {
            _relationalContext = relationalContext;
        }

        public async Task<string> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var entity = new ProductEntity
            {
                Id = Guid.NewGuid().ToString(),
                Description = request.Description,
                Name = request.Name,
                CategoryId = request.CategoryId,
                UnitPrice = request.UnitPrice
            };

            _relationalContext.Add(entity);

            await _relationalContext.SaveChangesAsync();

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
                UnitPrice = request.UnitPrice
            };

            _relationalContext.Update(entity);

            await _relationalContext.SaveChangesAsync();

            return Unit.Value;
        }

        public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            _relationalContext.Products.Remove(new ProductEntity { Id = request.Id });

            await _relationalContext.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
