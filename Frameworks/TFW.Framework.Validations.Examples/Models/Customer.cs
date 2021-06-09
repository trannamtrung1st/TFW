using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Framework.Validations.Examples.Models
{
    public class Customer : Person
    {
        public int Id { get; set; }
        public string Surname { get; set; }
        public string Forename { get; set; }
        public decimal Discount { get; set; }
        public Address Address { get; set; }
    }
}
