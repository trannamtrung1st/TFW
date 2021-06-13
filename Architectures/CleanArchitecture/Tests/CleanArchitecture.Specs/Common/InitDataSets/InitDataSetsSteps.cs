using CleanArchitecture.Specs.Common.Data;
using FluentAssertions;
using System;
using System.Linq;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace CleanArchitecture.Specs.Common.InitDataSets
{
    [Binding]
    public class InitDataSetsSteps
    {
        private string _dataSetKey;
        private InitCustomerModel[] _initCustomers;
        private InitEmployeeModel[] _initEmployees;
        private InitProductModel[] _initProducts;
        private InitSaleModel[] _initSales;

        [Given(@"the name of new dataset is ""(.*)""")]
        public void GivenTheNameOfNewDatasetIs(string dataSetKey)
        {
            _dataSetKey = dataSetKey;
        }

        [Given(@"the following Customers")]
        public void GivenTheFollowingCustomers(Table table)
        {
            _initCustomers = table.CreateSet<InitCustomerModel>().ToArray();
        }

        [Given(@"the following Employees")]
        public void GivenTheFollowingEmployees(Table table)
        {
            _initEmployees = table.CreateSet<InitEmployeeModel>().ToArray();
        }

        [Given(@"the following Products")]
        public void GivenTheFollowingProducts(Table table)
        {
            _initProducts = table.CreateSet<InitProductModel>().ToArray();
        }

        [Given(@"the following Sales")]
        public void GivenTheFollowingSales(Table table)
        {
            _initSales = table.CreateSet<InitSaleModel>().ToArray();
        }

        [When(@"init the dataset")]
        public void WhenInitTheDataset()
        {
            var initDataSetModel = new InitCleanArchitectureDataSetModel
            {
                Customers = _initCustomers,
                Employees = _initEmployees,
                Products = _initProducts,
                Sales = _initSales
            };

            DataSets.Set(_dataSetKey, initDataSetModel);
        }

        [Then(@"it should be added to the list of datasets successfully")]
        public void ThenItShouldBeAddedToTheListOfDatasetsSuccessfully()
        {
            var exists = DataSets.Contains(_dataSetKey);

            exists.Should().BeTrue();
        }
    }
}
