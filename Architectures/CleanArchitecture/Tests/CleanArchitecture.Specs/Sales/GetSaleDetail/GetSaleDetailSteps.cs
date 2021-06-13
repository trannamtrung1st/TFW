using Application.Sales.Queries.GetSaleDetail;
using System;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace CleanArchitecture.Specs.Sales.GetSaleDetail
{
    [Binding]
    public class GetSaleDetailSteps
    {
        private readonly IGetSaleDetailQuery _query;

        private SaleDetailModel _result;

        public GetSaleDetailSteps(IGetSaleDetailQuery query)
        {
            _query = query;
        }

        [When(@"I request the sale details for sale (.*)")]
        public async Task WhenIRequestTheSaleDetailsForSale(int saleId)
        {
            _result = await _query.ExecuteAsync(saleId);
        }

        [Then(@"the following sale details should be returned:")]
        public void ThenTheFollowingSaleDetailsShouldBeReturned(Table table)
        {
            table.CompareToInstance(_result);
        }
    }
}
