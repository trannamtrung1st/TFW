using NUnit.Framework;
using Application.Products.Queries.GetProductsList;
using System;
using System.Collections.Generic;
using System.Text;
using Domain.Products;
using System.Threading.Tasks;
using System.Linq;
using Application.Abstracts.Data;
using Moq;

namespace Application.Products.Queries.GetProductsList.Tests
{
    [TestFixture()]
    public class GetProductsListQueryTests
    {
        [SetUp]
        public void SetUp()
        {
        }

        [Test()]
        public async Task ExecuteAsyncTest()
        {
            var products = new Product[]
            {
                new Product {Id = 1, Name = "Abc", Price = 215},
                new Product {Id = 2, Name = "Xyz", Price = 152},
                new Product {Id = 3, Name = "AXyz", Price = 152.42},
                new Product {Id = 4, Name = "BXyz", Price = 152.12},
            };

            var productModels = products.Select(o => new ProductModel
            {
                Id = o.Id,
                Name = o.Name,
                UnitPrice = o.Price
            }).ToArray();

            var uowMock = new Mock<IUnitOfWork>();

            uowMock.Setup(o => o.ToArrayAsync(It.IsAny<IQueryable<ProductModel>>())).Returns((IQueryable<ProductModel> inp) =>
            {
                var expected = inp.Select(o => new ProductModel
                {
                    Id = o.Id,
                    Name = o.Name,
                    UnitPrice = o.UnitPrice
                }).ToArray();

                return Task.FromResult(expected);
            });

            var productRepoMock = new Mock<IRepository<Product>>();

            productRepoMock.Setup(o => o.Get()).Returns(products.AsQueryable());

            var query = new GetProductsListQuery(uowMock.Object, productRepoMock.Object);

            var results = await query.ExecuteAsync();

            for (var i = 0; i < productModels.Length; i++)
            {
                var expected = productModels[i];
                var actual = results[i];

                Assert.AreEqual(expected.Id, actual.Id);
                Assert.AreEqual(expected.Name, actual.Name);
                Assert.AreEqual(expected.UnitPrice, actual.UnitPrice);
            }
        }
    }
}