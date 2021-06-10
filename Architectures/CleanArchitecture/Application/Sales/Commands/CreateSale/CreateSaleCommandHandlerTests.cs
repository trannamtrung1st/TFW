using Application.Abstracts.Data;
using Application.Abstracts.Services;
using Application.Sales.Commands.CreateSale.Factories;
using Cross.Dates;
using Domain.Customers;
using Domain.Employees;
using Domain.Products;
using Domain.Sales;
using MediatR;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Sales.Commands.CreateSale
{
    [TestFixture]
    public class CreateSaleCommandHandlerTests
    {
        private IRequestHandler<CreateSaleCommand, Sale> _handler;
        private CreateSaleCommand _command;
        private Sale _sale;
        private Mock<IRepository<Sale>> _saleRepoMock;
        private Mock<IUnitOfWork> _uowMock;
        private Mock<IInventoryService> _inventoryServiceMock;

        private static readonly DateTime Date = new DateTime(2001, 2, 3);
        private const int CustomerId = 1;
        private const int EmployeeId = 2;
        private const int ProductId = 3;
        private const decimal UnitPrice = 1.23m;
        private const int Quantity = 4;

        [SetUp]
        public void SetUp()
        {
            var customer = new Customer
            {
                Id = CustomerId
            };

            var employee = new Employee
            {
                Id = EmployeeId
            };

            var product = new Product
            {
                Id = ProductId,
                Price = UnitPrice
            };

            _command = new CreateSaleCommand()
            {
                CustomerId = CustomerId,
                EmployeeId = EmployeeId,
                ProductId = ProductId,
                Quantity = Quantity
            };

            _sale = new Sale();

            var dateMock = new Mock<IDateService>();
            _uowMock = new Mock<IUnitOfWork>();
            _saleRepoMock = new Mock<IRepository<Sale>>();
            _inventoryServiceMock = new Mock<IInventoryService>();
            var customerRepoMock = new Mock<IRepository<Customer>>();
            var empRepoMock = new Mock<IRepository<Employee>>();
            var productRepoMock = new Mock<IRepository<Product>>();
            var factoryMock = new Mock<ISaleFactory>();

            dateMock.Setup(o => o.GetDate()).Returns(Date);

            _uowMock.Setup(o => o.SaveChangesAsync()).Returns(Task.FromResult(1));

            customerRepoMock.Setup(o => o.Get()).Returns(new[] { customer }.AsQueryable());
            empRepoMock.Setup(o => o.Get()).Returns(new[] { employee }.AsQueryable());
            productRepoMock.Setup(o => o.Get()).Returns(new[] { product }.AsQueryable());

            _saleRepoMock.Setup(o => o.Add(_sale)).Returns(_sale);

            factoryMock.Setup(p => p.Create(Date, customer, employee, product, Quantity))
                .Returns(_sale);

            _handler = new CreateSaleCommandHandler(dateMock.Object, _uowMock.Object,
                factoryMock.Object, _saleRepoMock.Object, customerRepoMock.Object,
                empRepoMock.Object, productRepoMock.Object, _inventoryServiceMock.Object);
        }

        [Test]
        public async Task TestExecuteShouldAddSaleToTheDatabase()
        {
            await _handler.Handle(_command, default);

            _saleRepoMock.Verify(p => p.Add(_sale), Times.Once);
        }

        [Test]
        public async Task TestExecuteShouldSaveChangesToDatabase()
        {
            await _handler.Handle(_command, default);

            _uowMock.Verify(p => p.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task TestExecuteShouldNotifyInventoryThatSaleOccurred()
        {
            await _handler.Handle(_command, default);

            _inventoryServiceMock.Verify(p => p.NotifySaleOcurred(ProductId, Quantity),
                Times.Once);
        }
    }
}