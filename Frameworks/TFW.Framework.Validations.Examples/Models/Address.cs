using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Framework.Validations.Examples.Models
{
    public class Address
    {
        public string Town { get; set; }
        public string Country { get; set; }
        public string Postcode { get; set; }

        public List<string> AddressLines { get; set; } = new List<string>();
        public Person ARandomGuy { get; set; }
    }
}
