using NUnit.Framework;
using Application.Products.Commands.RemoveProduct;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Application.Tests.Common.Data;
using Domain.Products;
using Moq;
using Application.Abstracts.Data;
using System.Linq;
using Cross.Tests;

namespace Application.Products.Commands.RemoveProduct.Tests
{
    [TestFixture()]
    public class RemoveProductCommandHandlerTests
    {
        private static object[] RemovedProductIds = new[]
        {
            new object[]{ DataSetKeys.Default, 1 },
            new object[]{ DataSetKeys.Default, 2 },
            new object[]{ DataSetKeys.Default, 3 },
        };

        private async Task<(dynamic expectedObj, Product removedProduct, Mock<IUnitOfWork> uowMock, Mock<IRepository<Product>> proRepoMock)>
            InitTestAsync(string dataSetKey, int productId)
        {
            var dSet = DataSets.Get(dataSetKey);

            var command = new RemoveProductCommand
            {
                ProductId = productId
            };

            var expectedObj = new
            {
                command
            };

            var uowMock = new Mock<IUnitOfWork>();

            uowMock.Setup(o => o.SaveChangesAsync(default)).ReturnsAsync(1);

            var proRepoMock = new Mock<IRepository<Product>>();

            proRepoMock.Setup(o => o.Get()).Returns(dSet.Products.AsQueryable());

            proRepoMock.Setup(o => o.Remove(It.Is<Product>(o => o.Id == productId)))
                .Returns((Product inp) =>
                {
                    inp.Deleted = true;
                    return inp;
                });

            var handler = new RemoveProductCommandHandler(uowMock.Object, proRepoMock.Object);

            var removedProduct = await handler.Handle(expectedObj.command, default);

            return (expectedObj, removedProduct, uowMock, proRepoMock);
        }

        [TestCaseSource(nameof(RemovedProductIds))]
        public async Task HandleTest(string dataSetKey, int productId)
        {
            var (_, removedProduct, _, _) = await InitTestAsync(dataSetKey, productId);

            Assert.AreEqual(true, removedProduct.Deleted);
        }

        [TestCaseSource(nameof(RemovedProductIds))]
        public async Task HandleShouldCallRemoveAndSaveChangesTest(string dataSetKey, int productId)
        {
            var (_, removedProduct, uowMock, proRepoMock) = await InitTestAsync(dataSetKey, productId);

            proRepoMock.Verify(o => o.Remove(It.Is<Product>(o => o.Id == productId)), Times.Once);

            uowMock.Verify(o => o.SaveChangesAsync(default), Times.Once);
        }
    }
}