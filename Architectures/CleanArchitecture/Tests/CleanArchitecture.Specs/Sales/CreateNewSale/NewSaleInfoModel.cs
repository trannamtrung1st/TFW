using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Specs.Sales.CreateNewSale
{
    public class NewSaleInfoModel
    {
        public string Customer { get; set; }
        public string Employee { get; set; }
        public string Product { get; set; }
        public int Quantity { get; set; }
    }
}
