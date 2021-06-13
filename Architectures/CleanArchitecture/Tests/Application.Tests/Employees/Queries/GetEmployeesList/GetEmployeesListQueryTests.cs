using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Domain.Employees;
using System.Threading.Tasks;
using System.Linq;
using Moq;
using Application.Abstracts.Data;
using Application.Tests.Common.Data;

namespace Application.Employees.Queries.GetEmployeesList.Tests
{
    [TestFixture()]
    public class GetEmployeesListQueryTests
    {
        [SetUp]
        public void SetUp()
        {
        }

        [Test()]
        public async Task ExecuteAsyncTest()
        {
            var dSet = DataSets.Get("default");

            var employeeModels = dSet.Employees.Select(o => new EmployeeModel
            {
                Id = o.Id,
                Name = o.Name
            }).ToArray();

            var expectedObj = new
            {
                employees = dSet.Employees,
                employeeModels
            };

            var uowMock = new Mock<IUnitOfWork>();

            uowMock.Setup(o => o.ToArrayAsync(It.IsAny<IQueryable<EmployeeModel>>())).Returns((IQueryable<EmployeeModel> inp) =>
            {
                return Task.FromResult(inp.ToArray());
            });

            var employeeRepoMock = new Mock<IRepository<Employee>>();

            employeeRepoMock.Setup(o => o.Get()).Returns(expectedObj.employees.AsQueryable());

            var query = new GetEmployeesListQuery(uowMock.Object, employeeRepoMock.Object);

            var results = await query.ExecuteAsync();

            Assert.AreEqual(expectedObj.employeeModels.Length, results.Length);

            for (var i = 0; i < expectedObj.employeeModels.Length; i++)
            {
                var expected = expectedObj.employeeModels[i];
                var actual = results[i];

                Assert.AreEqual(expected.Id, actual.Id);
                Assert.AreEqual(expected.Name, actual.Name);
            }
        }
    }
}