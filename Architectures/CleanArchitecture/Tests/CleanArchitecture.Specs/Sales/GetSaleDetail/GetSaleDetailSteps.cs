using Application.Sales.Queries.GetSaleDetail;
using CleanArchitecture.Specs.Common;
using CleanArchitecture.Specs.Common.Data;
using FluentAssertions;
using System;
using System.Linq;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace CleanArchitecture.Specs.Sales.GetSaleDetail
{
    [Binding]
    [Scope(Feature = "Get Sale Detail")]
    public class GetSaleDetailSteps
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly IGetSaleDetailQuery _query;

        private int _saleId;
        private SaleDetailModel _result;

        public GetSaleDetailSteps(ScenarioContext scenarioContext,
            IGetSaleDetailQuery query)
        {
            _scenarioContext = scenarioContext;
            _query = query;
        }

        [When(@"I request the sale detail for sale (.*)")]
        public async Task WhenIRequestTheSaleDetailForSale(int saleId)
        {
            _saleId = saleId;
            _result = await _query.ExecuteAsync(saleId);
        }

        [Then(@"the correct sale from dataset should be returned")]
        public void ThenTheCorrectSaleFromDatasetShouldBeReturned()
        {
            var expected = _scenarioContext.Get<CleanArchitectureDataSet>(ScenarioContextKeys.DataSet).Sales
                .Select(o => new SaleDetailModel
                {
                    Id = o.Id,
                    CustomerName = o.Customer.Name,
                    Date = o.Date,
                    EmployeeName = o.Employee.Name,
                    ProductName = o.Product.Name,
                    Quantity = o.Quantity,
                    TotalPrice = o.TotalPrice,
                    UnitPrice = o.UnitPrice
                }).Single(o => o.Id == _saleId);

            Compare(expected, _result);
        }

        [Then(@"the following sale detail should be returned:")]
        public void ThenTheFollowingSaleDetailShouldBeReturned(Table table)
        {
            table.CompareToInstance(_result);
        }

        public void Compare(SaleDetailModel expected, SaleDetailModel actual)
        {
            expected.CustomerName.Should().Be(actual.CustomerName);
            expected.Date.Should().Be(actual.Date);
            expected.EmployeeName.Should().Be(actual.EmployeeName);
            expected.Id.Should().Be(actual.Id);
            expected.ProductName.Should().Be(actual.ProductName);
            expected.Quantity.Should().Be(actual.Quantity);
            expected.TotalPrice.Should().Be(actual.TotalPrice);
            expected.UnitPrice.Should().Be(actual.UnitPrice);
        }
    }
}
