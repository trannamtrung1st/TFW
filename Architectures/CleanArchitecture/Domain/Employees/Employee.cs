using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Employees
{
    public class Employee : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
