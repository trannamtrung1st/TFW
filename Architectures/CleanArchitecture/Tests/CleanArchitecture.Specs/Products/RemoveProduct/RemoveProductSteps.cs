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
    [Scope(Feature = "Remove product")]
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

            _ = await _mediator.Send(new RemoveProductCommand
            {
                ProductId = _expectedRemovedProduct.Id
            });
        }

        [Then(@"that product is still in data store")]
        public void ThenThatProductIsStillInDataStore()
        {
            _actualRemovedProduct = _productRepository.IgnoreQueryFilters().Single(o => o.Id == _expectedRemovedProduct.Id);

            _actualRemovedProduct.Should().NotBeNull();
        }

        [Then(@"it is marked as deleted")]
        public void ThenItIsMarkedAsDeleted()
        {
            _actualRemovedProduct.Deleted.Should().BeTrue();
        }
    }
}
