using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TFW.Framework.CQRSExamples.Entities.Relational;

namespace TFW.Framework.CQRSExamples.Models.Command
{
    public class ProductCategoryCommandHandler : IRequestHandler<CreateProductCategoryCommand, string>,
        IRequestHandler<UpdateProductCategoryCommand>,
        IRequestHandler<DeleteProductCategoryCommand>
    {
        private readonly RelationalContext _relationalContext;

        public ProductCategoryCommandHandler(
            RelationalContext relationalContext)
        {
            _relationalContext = relationalContext;
        }

        public async Task<string> Handle(CreateProductCategoryCommand request, CancellationToken cancellationToken)
        {
            var entity = new ProductCategoryEntity
            {
                Id = Guid.NewGuid().ToString(),
                Description = request.Description,
                Name = request.Name
            };

            _relationalContext.Add(entity);

            await _relationalContext.SaveChangesAsync();

            return entity.Id;
        }

        public async Task<Unit> Handle(UpdateProductCategoryCommand request, CancellationToken cancellationToken)
        {
            var entity = new ProductCategoryEntity
            {
                Id = request.Id,
                Description = request.Description,
                Name = request.Name
            };

            _relationalContext.Update(entity);

            await _relationalContext.SaveChangesAsync();

            return Unit.Value;
        }

        public async Task<Unit> Handle(DeleteProductCategoryCommand request, CancellationToken cancellationToken)
        {
            _relationalContext.ProductCategories.Remove(new ProductCategoryEntity { Id = request.Id });

            await _relationalContext.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
