using Domain.Common;
using Domain.Customers;
using Domain.Employees;
using Domain.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Sales
{
    public class Sale : IEntity
    {
        public Sale() { }

        public Sale(DateTime date, Customer customer, Employee employee, Product product, int quantity)
        {
            Date = date;
            Customer = customer;
            Employee = employee;
            Product = product;
            UnitPrice = Product.Price;
            Quantity = quantity;
            // Note: Total price is calculated in domain logic
        }

        private int _quantity;
        private double _totalPrice;
        private double _unitPrice;

        public int Id { get; set; }

        public DateTime Date { get; set; }

        public Customer Customer { get; set; }

        public Employee Employee { get; set; }

        public Product Product { get; set; }

        public double UnitPrice
        {
            get { return _unitPrice; }
            set
            {
                _unitPrice = value;

                UpdateTotalPrice();
            }
        }

        public int Quantity
        {
            get { return _quantity; }
            set
            {
                _quantity = value;

                UpdateTotalPrice();
            }
        }

        public double TotalPrice
        {
            get { return _totalPrice; }
            private set { _totalPrice = value; }
        }

        private void UpdateTotalPrice()
        {
            _totalPrice = _unitPrice * _quantity;
        }
    }
}
