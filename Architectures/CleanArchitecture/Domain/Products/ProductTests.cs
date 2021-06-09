using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Products
{
    [TestFixture]
    public class ProductTests
    {
        private Product _product;
        private const int Id = 1;
        private const string Name = "Test";

        [SetUp]
        public void Setup()
        {
            _product = new Product();
        }

        [Test]
        public void TestSetAndGetId()
        {
            _product.Id = Id;

            Assert.AreEqual(_product.Id, Id);
        }

        [Test]
        public void TestSetAndGetName()
        {
            _product.Name = Name;

            Assert.AreEqual(_product.Name, Name);
        }
    }
}
