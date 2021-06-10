using Domain.Customers;
using Domain.Employees;
using Domain.Products;
using Domain.Sales;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Sales.Commands.CreateSale.Factories
{
    public interface ISaleFactory
    {
        Sale Create(DateTime date, Customer customer, Employee employee, Product product, int quantity);
    }
}
