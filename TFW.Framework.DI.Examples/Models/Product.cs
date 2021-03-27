using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Framework.DI.Examples.Models
{
    public class Product
    {
        public string Name { get; set; }

        public Product(string name)
        {
            Name = name;
        }
    }
}
