using CleanArchitecture.Specs.Common.Data;
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

        public ManageDataSetsSteps(ScenarioContext scenarioContext,
            ISpecDbMigrator specDbMigrator)
        {
            _scenarioContext = scenarioContext;
            _specDbMigrator = specDbMigrator;
        }

        [Given(@"""(.*)"" dataset")]
        public void GivenDataset(string dataSetKey)
        {
            _scenarioContext[ScenarioContextKeys.DataSet] = DataSets.Get(dataSetKey);
        }

        [Given(@"products with following names are marked as deleted")]
        public void GivenProductsWithFollowingNamesAreMarkedAsDeleted(Table table)
        {
            var dSet = _scenarioContext.Get<CleanArchitectureDataSet>(ScenarioContextKeys.DataSet);

            var deletedNames = table.Rows.SelectMany(o => o.Values).ToArray();
            var deletedProducts = dSet.Products.Where(o => deletedNames.Contains(o.Name)).ToArray();

            foreach (var product in deletedProducts)
                product.Deleted = true;
        }

        [Given(@"dataset is created")]
        public async Task GivenDatasetIsCreated()
        {
            var dSet = _scenarioContext.Get<CleanArchitectureDataSet>(ScenarioContextKeys.DataSet);

            await _specDbMigrator.DropAsync();
            await _specDbMigrator.InitAsync(dSet);
        }
    }
}
