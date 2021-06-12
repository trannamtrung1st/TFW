using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Domain.Sales;
using Domain.Customers;
using Domain.Employees;
using Domain.Products;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Cross.Dates;
using Application.Abstracts.Data;
using Application.Sales.Commands.CreateSale.Factories;
using Application.Abstracts.Services;
using System.Threading;

namespace Application.Sales.Commands.CreateSale.Tests
{
    [TestFixture()]
    public class CreateSaleCommandHandlerTests
    {
        private Customer[] _customers;
        private Employee[] _employees;
        private Product[] _products;

        [SetUp]
        public void Setup()
        {
            var range = Enumerable.Range(1, 10);

            _customers = range.Select(i => new Customer
            {
                Id = i,
                Name = $"Customer {i}"
            }).ToArray();

            _employees = range.Select(i => new Employee
            {
                Id = i,
                Name = $"Employee {i}"
            }).ToArray();

            _products = range.Select(i => new Product
            {
                Id = i,
                Name = $"Product {i}",
                Price = i * new Random().NextDouble() * 100
            }).ToArray();
        }

        private static object[] CommandArgs = new[]
        {
            new object[]{ 1, 4, 5, 14 },
            new object[]{ 6, 3, 1, 24 },
            new object[]{ 2, 7, 9, 404 },
        };

        private async Task<(dynamic expectedObj, Sale actual, Mock<IRepository<Sale>> saleRepoMock,
            Mock<IUnitOfWork> uowMock, Mock<IInventoryService> inventoryServiceMock)>
            InitHandleTestAsync(int customerId, int employeeId, int productId, int quantity)
        {
            var selectedCustomer = _customers.Single(o => o.Id == customerId);
            var selectedEmployee = _employees.Single(o => o.Id == employeeId);
            var selectedProduct = _products.Single(o => o.Id == productId);

            var createSaleCommand = new CreateSaleCommand
            {
                CustomerId = selectedCustomer.Id,
                EmployeeId = selectedEmployee.Id,
                ProductId = selectedProduct.Id,
                Quantity = quantity
            };

            var now = DateTime.Now;

            var createdSale = new Sale(now, selectedCustomer, selectedEmployee, selectedProduct, quantity)
            {
                Id = 1
            };

            var expectedObj = new
            {
                customers = _customers,
                employees = _employees,
                products = _products,
                createSaleCommand,
                createdSale
            };

            var dateServiceMock = new Mock<IDateService>();

            dateServiceMock.Setup(o => o.GetDate()).Returns(expectedObj.createdSale.Date);

            var cusRepoMock = new Mock<IRepository<Customer>>();
            var empRepoMock = new Mock<IRepository<Employee>>();
            var prodRepoMock = new Mock<IRepository<Product>>();

            cusRepoMock.Setup(o => o.Get()).Returns(expectedObj.customers.AsQueryable());
            empRepoMock.Setup(o => o.Get()).Returns(expectedObj.employees.AsQueryable());
            prodRepoMock.Setup(o => o.Get()).Returns(expectedObj.products.AsQueryable());

            var saleRepoMock = new Mock<IRepository<Sale>>();

            Sale actualSaleTemp = null;

            saleRepoMock.Setup(o => o.Add(It.IsAny<Sale>())).Returns((Sale inp) =>
            {
                actualSaleTemp = inp;
                return inp;
            });

            var uowMock = new Mock<IUnitOfWork>();

            uowMock.Setup(o => o.SaveChangesAsync(default)).Returns(() =>
            {
                actualSaleTemp.Id = expectedObj.createdSale.Id;
                return Task.FromResult(1);
            });

            var saleFactoryMock = new Mock<ISaleFactory>();

            saleFactoryMock.Setup(o => o.Create(expectedObj.createdSale.Date,
                    expectedObj.createdSale.Customer, expectedObj.createdSale.Employee, expectedObj.createdSale.Product,
                    expectedObj.createdSale.Quantity))
                .Returns(new Sale(expectedObj.createdSale.Date, expectedObj.createdSale.Customer,
                    expectedObj.createdSale.Employee, expectedObj.createdSale.Product, expectedObj.createdSale.Quantity));

            var inventoryServiceMock = new Mock<IInventoryService>();

            inventoryServiceMock.Setup(o => o.NotifySaleOcurred(expectedObj.createSaleCommand.ProductId,
                expectedObj.createSaleCommand.Quantity));

            var commandHandler = new CreateSaleCommandHandler(dateServiceMock.Object,
                uowMock.Object,
                saleFactoryMock.Object,
                saleRepoMock.Object,
                cusRepoMock.Object,
                empRepoMock.Object,
                prodRepoMock.Object,
                inventoryServiceMock.Object);

            var actual = await commandHandler.Handle(expectedObj.createSaleCommand, default);

            return (expectedObj, actual, saleRepoMock, uowMock, inventoryServiceMock);
        }

        [TestCaseSource(nameof(CommandArgs))]
        public async Task HandleTest(int customerId, int employeeId, int productId, int quantity)
        {
            var (expectedObj, actual, _, _, _) =
                await InitHandleTestAsync(customerId, employeeId, productId, quantity);

            var expected = expectedObj.createdSale;
            Assert.AreEqual(expected.Customer, actual.Customer);
            Assert.AreEqual(expected.Date, actual.Date);
            Assert.AreEqual(expected.Employee, actual.Employee);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Product, actual.Product);
            Assert.AreEqual(expected.Quantity, actual.Quantity);
            Assert.AreEqual(expected.TotalPrice, actual.TotalPrice);
            Assert.AreEqual(expected.UnitPrice, actual.UnitPrice);
        }

        [TestCaseSource(nameof(CommandArgs))]
        public async Task HandleShouldSaveChangesTest(int customerId, int employeeId, int productId, int quantity)
        {
            var (expectedObj, actual, saleRepoMock, uowMock, inventoryServiceMock) =
                await InitHandleTestAsync(customerId, employeeId, productId, quantity);

            saleRepoMock.Verify(o => o.Add(actual), Times.Once);

            uowMock.Verify(o => o.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestCaseSource(nameof(CommandArgs))]
        public async Task HandleShouldNotifyInventoryTest(int customerId, int employeeId, int productId, int quantity)
        {
            var (expectedObj, actual, saleRepoMock, uowMock, inventoryServiceMock) =
                await InitHandleTestAsync(customerId, employeeId, productId, quantity);

            inventoryServiceMock.Verify(o => o.NotifySaleOcurred(productId, quantity), Times.Once);
        }
    }
}