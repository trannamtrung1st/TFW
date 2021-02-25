using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Framework.Validations.Examples.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Surname { get; set; }
        public string Forename { get; set; }
        public decimal Discount { get; set; }
        public string Address { get; set; }
    }
}
