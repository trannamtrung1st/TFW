using Application.Employees.Queries.GetEmployeesList;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace CleanArchitecture.Specs.Employees.GetEmployeesList
{
    [Binding]
    public class GetEmployeesListSteps
    {
        private readonly IGetEmployeesListQuery _query;

        private EmployeeModel[] _results;

        public GetEmployeesListSteps(IGetEmployeesListQuery query)
        {
            _query = query;
        }

        [When(@"I request a list of employees")]
        public async Task WhenIRequestAListOfEmployees()
        {
            _results = await _query.ExecuteAsync();
        }

        [Then(@"the following employees should be returned:")]
        public void ThenTheFollowingEmployeesShouldBeReturned(Table table)
        {
            var expectedResults = table.CreateSet<EmployeeModel>().ToArray();

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
