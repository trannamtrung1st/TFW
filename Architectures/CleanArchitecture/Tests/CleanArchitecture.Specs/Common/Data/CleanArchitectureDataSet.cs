using Domain.Customers;
using Domain.Employees;
using Domain.Products;
using Domain.Sales;
using System.Collections.Generic;

namespace CleanArchitecture.Specs.Common.Data
{
    public class CleanArchitectureDataSet
    {
        public IEnumerable<Customer> Customers { get; set; }
        public IEnumerable<Employee> Employees { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Sale> Sales { get; set; }
    }
}
