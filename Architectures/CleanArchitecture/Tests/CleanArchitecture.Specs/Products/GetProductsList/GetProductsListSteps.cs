using Application.Products.Queries.GetProductsList;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace CleanArchitecture.Specs.Products.GetProductsList
{
    [Binding]
    public class GetProductsListSteps
    {
        private readonly IGetProductsListQuery _query;

        private ProductModel[] _results;

        public GetProductsListSteps(IGetProductsListQuery query)
        {
            _query = query;
        }

        [When(@"I request a list of products")]
        public async Task WhenIRequestAListOfProducts()
        {
            _results = await _query.ExecuteAsync();
        }

        [Then(@"the following products should be returned:")]
        public void ThenTheFollowingProductsShouldBeReturned(Table table)
        {
            var expectedResults = table.CreateSet<ProductModel>().ToArray();

            var expectedObj = new
            {
                expectedResults
            };

            Assert.AreEqual(expectedObj.expectedResults.Length, _results.Length);

            for (var i = 0; i < expectedObj.expectedResults.Length; i++)
            {
                var expected = expectedObj.expectedResults[i];
                var actual = _results[i];

                Assert.AreEqual(expected.Id, actual.Id);
                Assert.AreEqual(expected.Name, actual.Name);
                Assert.AreEqual(expected.UnitPrice, actual.UnitPrice);
            }
        }
    }
}
