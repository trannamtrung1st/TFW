using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Employees
{
    [TestFixture]
    public class EmployeeTests
    {
        private Employee _employee;
        private const int Id = 1;
        private const string Name = "Test";

        [SetUp]
        public void Setup()
        {
            _employee = new Employee();
        }

        [Test]
        public void TestSetAndGetId()
        {
            _employee.Id = Id;

            Assert.AreEqual(_employee.Id, Id);
        }

        [Test]
        public void TestSetAndGetName()
        {
            _employee.Name = Name;

            Assert.AreEqual(_employee.Name, Name);
        }
    }
}
