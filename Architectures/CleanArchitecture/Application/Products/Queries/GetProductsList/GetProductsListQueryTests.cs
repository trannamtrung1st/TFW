using Application.Abstracts.Data;
using Domain.Products;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Products.Queries.GetProductsList
{
    [TestFixture]
    public class GetProductsListQueryTests
    {
        private GetProductsListQuery _query;
        private Product _product;

        private const int Id = 1;
        private const string Name = "Product 1";
        private const double Price = 100;

        [SetUp]
        public void SetUp()
        {
            _product = new Product()
            {
                Id = Id,
                Name = Name,
                Price = Price
            };

            var productArr = new[] { _product };
            var productModelArr = new[]
            {
                new ProductModel
                {
                    Id = _product.Id,
                    Name = _product.Name,
                    UnitPrice = _product.Price
                }
            };

            var queryable = productModelArr.AsQueryable();
            var repoMock = new Mock<IRepository<Product>>();
            var uowMock = new Mock<IUnitOfWork>();

            repoMock.Setup(p => p.Get())
                .Returns(() => productArr.AsQueryable());

            uowMock.Setup(p => p.ToArrayAsync(queryable))
                .Returns(() => Task.FromResult(productModelArr));

            _query = new GetProductsListQuery(uowMock.Object, repoMock.Object);
        }

        [Test]
        public async Task TestExecuteShouldReturnListOfProducts()
        {
            var results = await _query.ExecuteAsync();

            var result = results.Single();

            Assert.AreEqual(result.Id, Id);
            Assert.AreEqual(result.Name, Name);
            Assert.AreEqual(result.UnitPrice, Price);
        }
    }
}
