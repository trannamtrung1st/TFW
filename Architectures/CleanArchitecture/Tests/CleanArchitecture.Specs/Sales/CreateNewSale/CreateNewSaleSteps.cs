using Application.Abstracts.Data;
using Application.Abstracts.Services;
using Application.Sales.Commands.CreateSale;
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
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace CleanArchitecture.Specs.Sales.CreateNewSale
{
    [Binding]
    public class CreateNewSaleSteps
    {
        private readonly IMediator _mediator;
        private readonly IInventoryService _mockInventoryService;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<Employee> _employeeRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Sale> _saleRepository;

        private CreateSaleCommand _command;
        private Sale _returnSale;

        public CreateNewSaleSteps(IMediator mediator,
            IRepository<Customer> customerRepository,
            IRepository<Employee> employeeRepository,
            IRepository<Product> productRepository,
            IRepository<Sale> saleRepository,
            IInventoryService mockInventoryService)
        {
            _mediator = mediator;
            _mockInventoryService = mockInventoryService;
            _customerRepository = customerRepository;
            _employeeRepository = employeeRepository;
            _productRepository = productRepository;
            _saleRepository = saleRepository;
        }

        [Given(@"the following sale info:")]
        public void GivenTheFollowingSaleInfo(Table table)
        {
            var newSaleInfo = table.CreateInstance<NewSaleInfoModel>();

            var customer = _customerRepository.Get().Single(o => o.Name == newSaleInfo.Customer);
            var employee = _employeeRepository.Get().Single(o => o.Name == newSaleInfo.Employee);
            var product = _productRepository.Get().Single(o => o.Name == newSaleInfo.Product);

            _command = new CreateSaleCommand
            {
                CustomerId = customer.Id,
                EmployeeId = employee.Id,
                ProductId = product.Id,
                Quantity = newSaleInfo.Quantity
            };
        }

        [When(@"I create a sale")]
        public async Task WhenICreateASale()
        {
            _returnSale = await _mediator.Send(_command);
        }

        [Then(@"the following sales record should be recorded:")]
        public void ThenTheFollowingSalesRecordShouldBeRecorded(Table table)
        {
            var expectedDate = DateTime.Now.Date;

            var actual = _saleRepository.Get().Select(o => new RecordedSaleInfoModel
            {
                Customer = o.Customer.Name,
                Date = o.Date,
                Employee = o.Employee.Name,
                Product = o.Product.Name,
                Quantity = o.Quantity,
                TotalPrice = o.TotalPrice,
                UnitPrice = o.UnitPrice
            }).ToArray().Last();

            Assert.AreEqual(expectedDate, actual.Date);

            var newTable = new Table(table.Header.ToArray());
            var values = table.Rows[0].Values.ToArray();
            values[0] = expectedDate.ToString();
            newTable.AddRow(values);

            newTable.CompareToInstance(actual);
        }

        [Then(@"the following sale-occurred notification should be sent to the inventory system:")]
        public void ThenTheFollowingSale_OccurredNotificationShouldBeSentToTheInventorySystem(Table table)
        {
            var expectedObj = table.CreateInstance<InventoryNotificationModel>();

            Mock.Get(_mockInventoryService)
                .Verify(o => o.NotifySaleOcurred(expectedObj.ProductId, expectedObj.Quantity), Times.Once);
        }
    }
}
