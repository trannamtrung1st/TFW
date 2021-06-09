using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Customers
{
    public class Customer : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
