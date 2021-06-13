using Application.Abstracts.Data;
using CleanArchitecture.Specs.Common.Data;
using Domain.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace CleanArchitecture.Specs.Common.ManageDataSets
{
    [Binding]
    public sealed class ManageDataSetsSteps
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly ISpecDbMigrator _specDbMigrator;
        private readonly IRepository<Product> _productRepository;
        private readonly IUnitOfWork _uow;

        public ManageDataSetsSteps(ScenarioContext scenarioContext,
            ISpecDbMigrator specDbMigrator,
            IRepository<Product> productRepository,
            IUnitOfWork uow)
        {
            _scenarioContext = scenarioContext;
            _specDbMigrator = specDbMigrator;
            _productRepository = productRepository;
            _uow = uow;
        }

        [Given(@"""(.*)"" dataset")]
        public async Task GivenDataset(string dataSetKey)
        {
            await _specDbMigrator.InitAsync(dataSetKey);
        }

        [Given(@"products with following names are marked as deleted")]
        public async Task GivenProductsWithFollowingNamesAreMarkedAsDeleted(Table table)
        {
            var deletedNames = table.Rows.SelectMany(o => o.Values).ToArray();

            var deletedProducts = _productRepository.Get().Where(o => deletedNames.Contains(o.Name))
                .ToArray();

            foreach (var product in deletedProducts)
                _productRepository.Remove(product);

            await _uow.SaveChangesAsync();
        }
    }
}
