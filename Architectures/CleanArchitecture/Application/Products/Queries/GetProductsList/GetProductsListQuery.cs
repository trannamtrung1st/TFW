﻿using Application.Abstracts.Data;
using Domain.Products;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Products.Queries.GetProductsList
{
    public class GetProductsListQuery : IGetProductsListQuery
    {
        private readonly IUnitOfWork _uow;
        private readonly IRepository<Product> _productRepository;

        public GetProductsListQuery(IUnitOfWork uow,
            IRepository<Product> productRepository)
        {
            _uow = uow;
            _productRepository = productRepository;
        }

        public async Task<ProductModel[]> ExecuteAsync()
        {
            var query = _productRepository.Get()
                .Select(p => new ProductModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    UnitPrice = p.Price
                });

            return await _uow.ToArrayAsync(query);
        }
    }
}
