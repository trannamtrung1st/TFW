using CleanArchitecture.Specs.Common.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace CleanArchitecture.Specs.Common
{
    [Binding]
    public class GlobalHooks
    {
        private ScenarioContext _scenarioContext;
        private readonly ISpecDbMigrator _specDbMigrator;

        public GlobalHooks(ScenarioContext scenarioContext,
            ISpecDbMigrator specDbMigrator)
        {
            _scenarioContext = scenarioContext;
            _specDbMigrator = specDbMigrator;
        }

        [BeforeScenario]
        public async Task SetUp()
        {
            Console.WriteLine($"Before {_scenarioContext}");

            await _specDbMigrator.DropAsync();
        }

        [AfterScenario]
        public async Task TearDown()
        {
            Console.WriteLine($"After {_scenarioContext}");

            await _specDbMigrator.DropAsync();
        }
    }
}
