using Domain.Customers;
using Domain.Employees;
using Domain.Products;
using Domain.Sales;
using System;

namespace Application.Sales.Commands.CreateSale.Factories
{
    public class SaleFactory : ISaleFactory
    {
        public Sale Create(DateTime date, Customer customer, Employee employee, Product product, int quantity)
        {
            return new Sale(date, customer, employee, product, quantity);
        }
    }
}
