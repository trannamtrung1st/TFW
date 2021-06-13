using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Specs.Common.InitDataSets
{
    public class InitCleanArchitectureDataSetModel
    {
        public IEnumerable<InitCustomerModel> Customers { get; set; }
        public IEnumerable<InitEmployeeModel> Employees { get; set; }
        public IEnumerable<InitProductModel> Products { get; set; }
        public IEnumerable<InitSaleModel> Sales { get; set; }
    }
}
