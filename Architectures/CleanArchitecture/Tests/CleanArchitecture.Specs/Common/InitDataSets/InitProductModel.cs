using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Specs.Common.InitDataSets
{
    public class InitProductModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double UnitPrice { get; set; }
        public bool Deleted { get; set; }
    }
}
