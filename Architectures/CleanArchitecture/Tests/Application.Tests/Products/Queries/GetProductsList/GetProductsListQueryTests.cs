using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Domain.Products;
using System.Threading.Tasks;
using System.Linq;
using Application.Abstracts.Data;
using Moq;
using Application.Tests.Common.Data;

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
            var dSet = DataSets.Get("default");

            var productModels = dSet.Products.Select(o => new ProductModel
            {
                Id = o.Id,
                Name = o.Name,
                UnitPrice = o.Price
            }).ToArray();

            var expectedObj = new
            {
                products = dSet.Products,
                productModels
            };

            var uowMock = new Mock<IUnitOfWork>();

            uowMock.Setup(o => o.ToArrayAsync(It.IsAny<IQueryable<ProductModel>>())).Returns((IQueryable<ProductModel> inp) =>
            {
                return Task.FromResult(inp.ToArray());
            });

            var productRepoMock = new Mock<IRepository<Product>>();

            productRepoMock.Setup(o => o.Get()).Returns(expectedObj.products.AsQueryable());

            var query = new GetProductsListQuery(uowMock.Object, productRepoMock.Object);

            var results = await query.ExecuteAsync();

            Assert.AreEqual(expectedObj.productModels.Length, results.Length);

            for (var i = 0; i < expectedObj.productModels.Length; i++)
            {
                var expected = expectedObj.productModels[i];
                var actual = results[i];

                Assert.AreEqual(expected.Id, actual.Id);
                Assert.AreEqual(expected.Name, actual.Name);
                Assert.AreEqual(expected.UnitPrice, actual.UnitPrice);
            }
        }
    }
}