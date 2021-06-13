using Persistence.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace CleanArchitecture.Specs.Common
{
    [Binding]
    public class GlobalHooks
    {
        private ScenarioContext _scenarioContext;
        private readonly IDbMigrator _dbMigrator;

        public GlobalHooks(ScenarioContext scenarioContext, IDbMigrator dbMigrator)
        {
            _scenarioContext = scenarioContext;
            _dbMigrator = dbMigrator;

        }

        [BeforeScenario]
        public async Task SetUp()
        {
            await _dbMigrator.DropAsync();
            await _dbMigrator.InitAsync();
        }


        [AfterScenario]
        public async Task TearDown()
        {
            await _dbMigrator.DropAsync();
        }
    }
}
