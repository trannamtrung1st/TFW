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
        public async Task GivenDataset(string dataSetKey)
        {
            await _specDbMigrator.InitAsync(dataSetKey);
        }
    }
}
