using Application.Employees.Queries.GetEmployeesList;
using CleanArchitecture.Specs.Common;
using CleanArchitecture.Specs.Common.Data;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace CleanArchitecture.Specs.Employees.GetEmployeesList
{
    [Binding]
    [Scope(Feature = "Get Employees List")]
    public class GetEmployeesListSteps
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly IGetEmployeesListQuery _query;

        private EmployeeModel[] _results;

        public GetEmployeesListSteps(ScenarioContext scenarioContext,
            IGetEmployeesListQuery query)
        {
            _scenarioContext = scenarioContext;
            _query = query;
        }

        [When(@"I request a list of employees")]
        public async Task WhenIRequestAListOfEmployees()
        {
            _results = await _query.ExecuteAsync();
        }

        [Then(@"the employees dataset should be returned")]
        public void ThenTheEmployeesDatasetShouldBeReturned()
        {
            var expectedResults = _scenarioContext.Get<CleanArchitectureDataSet>(ScenarioContextKeys.DataSet).Employees
                .Select(o => new EmployeeModel
                {
                    Id = o.Id,
                    Name = o.Name
                }).ToArray();

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
            }
        }
    }
}
