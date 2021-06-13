using Application.Abstracts.Data;
using Application.Products.Commands.RemoveProduct;
using Domain.Products;
using FluentAssertions;
using MediatR;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace CleanArchitecture.Specs.Products.RemoveProduct
{
    [Binding]
    public class RemoveProductSteps
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IMediator _mediator;

        private Product _expectedRemovedProduct;
        private Product _actualRemovedProduct;

        public RemoveProductSteps(IRepository<Product> productRepository,
            IMediator mediator)
        {
            _productRepository = productRepository;
            _mediator = mediator;
        }

        [When(@"remove the product ""(.*)""")]
        public async Task WhenRemoveTheProduct(string productName)
        {
            _expectedRemovedProduct = _productRepository.Get().Single(o => o.Name == productName);

            _actualRemovedProduct = await _mediator.Send(new RemoveProductCommand
            {
                ProductId = _expectedRemovedProduct.Id
            });
        }

        [Then(@"that product is marked as deleted")]
        public void ThenThatProductIsMarkedAsDeleted()
        {
            _actualRemovedProduct.Deleted.Should().BeTrue();
        }

        [Then(@"it is still stored in data store")]
        public void ThenItIsStillStoredInDataStore()
        {
            var removedProduct = _productRepository.IgnoreQueryFilters().Single(o => o.Name == _expectedRemovedProduct.Name);

            removedProduct.Id.Should().Be(_expectedRemovedProduct.Id);
        }
    }
}
