using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Products
{
    public class Product : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public bool Deleted { get; set; }
    }
}
