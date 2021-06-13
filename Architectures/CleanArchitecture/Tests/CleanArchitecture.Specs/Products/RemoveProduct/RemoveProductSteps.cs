using Application.Abstracts.Data;
using Domain.Products;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Linq;
using TechTalk.SpecFlow;

namespace CleanArchitecture.Specs.Products.RemoveProduct
{
    [Binding]
    public class RemoveProductSteps
    {
        private readonly IRepository<Product> _productRepository;

        private Product _removedProduct;

        public RemoveProductSteps(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        [When(@"remove the product ""(.*)""")]
        public void WhenRemoveTheProduct(string productName)
        {
            _removedProduct = _productRepository.Get().Single(o => o.Name == productName);

            // [TODO] send remove command
        }

        [Then(@"that product is marked as deleted")]
        public void ThenThatProductIsMarkedAsDeleted()
        {
            Assert.Fail();
        }

        [Then(@"it is still stored in data store")]
        public void ThenItIsStillStoredInDataStore()
        {
            var removedProduct = _productRepository.Get().Single(o => o.Name == _removedProduct.Name);

            removedProduct.Id.Should().Be(_removedProduct.Id);
        }
    }
}
