using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Specs.Sales.CreateNewSale
{
    public class RecordedSaleInfoModel
    {
        public DateTime Date { get; set; }
        public string Customer { get; set; }
        public string Employee { get; set; }
        public string Product { get; set; }
        public double UnitPrice { get; set; }
        public int Quantity { get; set; }
        public double TotalPrice { get; set; }
    }
}
