using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Customers
{
    [TestFixture]
    public class CustomerTests
    {
        private Customer _customer;
        private const int Id = 1;
        private const string Name = "Test";

        [SetUp]
        public void Setup()
        {
            _customer = new Customer();
        }

        [Test]
        public void TestSetAndGetId()
        {
            _customer.Id = Id;

            Assert.AreEqual(_customer.Id, Id);
        }

        [Test]
        public void TestSetAndGetName()
        {
            _customer.Name = Name;

            Assert.AreEqual(_customer.Name, Name);
        }
    }
}
