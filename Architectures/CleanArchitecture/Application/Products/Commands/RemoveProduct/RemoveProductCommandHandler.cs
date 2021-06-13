using Application.Abstracts.Data;
using Domain.Products;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Products.Commands.RemoveProduct
{
    public class RemoveProductCommandHandler : IRequestHandler<RemoveProductCommand, Product>
    {
        private readonly IUnitOfWork _uow;
        private readonly IRepository<Product> _productRepository;

        public RemoveProductCommandHandler(IUnitOfWork uow,
            IRepository<Product> productRepository)
        {
            _uow = uow;
            _productRepository = productRepository;
        }

        public async Task<Product> Handle(RemoveProductCommand request, CancellationToken cancellationToken)
        {
            var product = _productRepository.Get().Single(o => o.Id == request.ProductId);

            _productRepository.Remove(product);

            await _uow.SaveChangesAsync();

            return product;
        }
    }
}
