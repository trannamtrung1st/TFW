using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Products.Queries.GetProductsList
{
    public class ProductModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal UnitPrice { get; set; }
    }
}
