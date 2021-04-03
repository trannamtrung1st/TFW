using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TFW.Framework.DI.WebExamples.Models;

namespace TFW.Framework.DI.WebExamples.Builders
{
    public interface IProductBuilder
    {
        IProductBuilder Id(int id);
        IProductBuilder Name(string name);
        Product Build();
    }

    public class MyProductBuilder : IProductBuilder
    {
        private readonly Product _product;

        public MyProductBuilder()
        {
            _product = new Product();
        }

        public Product Build()
        {
            return _product;
        }

        public IProductBuilder Id(int id)
        {
            _product.Id = id;
            return this;
        }

        public IProductBuilder Name(string name)
        {
            _product.Name = "Name: " + name;
            return this;
        }
    }
}
