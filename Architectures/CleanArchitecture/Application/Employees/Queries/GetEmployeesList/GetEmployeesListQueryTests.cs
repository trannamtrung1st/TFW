using Application.Data;
using Domain.Employees;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Employees.Queries.GetEmployeesList
{
    [TestFixture]
    public class GetEmployeesListQueryTests
    {
        private GetEmployeesListQuery _query;
        private Employee _employee;

        private const int Id = 1;
        private const string Name = "Employee 1";

        [SetUp]
        public void SetUp()
        {
            _employee = new Employee()
            {
                Id = Id,
                Name = Name
            };

            var employeeArr = new[] { _employee };
            var employeeModelArr = new[]
            {
                new EmployeeModel
                {
                    Id = _employee.Id,
                    Name = _employee.Name
                }
            };

            var queryable = employeeModelArr.AsQueryable();
            var repoMock = new Mock<IRepository<Employee>>();

            repoMock.Setup(p => p.Get())
                .Returns(() => employeeArr.AsQueryable());

            repoMock.Setup(p => p.ToArrayAsync(queryable))
                .Returns(() => Task.FromResult(employeeModelArr));

            _query = new GetEmployeesListQuery(repoMock.Object);
        }

        [Test]
        public async Task TestExecuteShouldReturnListOfEmployees()
        {
            var results = await _query.ExecuteAsync();

            var result = results.Single();

            Assert.AreEqual(result.Id, Id);
            Assert.AreEqual(result.Name, Name);
        }
    }
}
