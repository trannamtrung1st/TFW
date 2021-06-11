﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Sales.Queries.GetSaleDetail
{
    public class SaleDetailModel
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string CustomerName { get; set; }

        public string EmployeeName { get; set; }

        public string ProductName { get; set; }

        public double UnitPrice { get; set; }

        public int Quantity { get; set; }

        public double TotalPrice { get; set; }
    }
}
