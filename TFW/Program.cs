using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace TFW
{
    public class A
    {
        public string Name { get; set; }
        public B B { get; set; }
        public List<B> ListB { get; set; }
    }

    public class B
    {
        public string Name { get; set; }
    }
    class Program
    {

        static void Main(string[] args)
        {
            var list = new List<A> { new A
            {
                B = new B{Name="1"},
                Name = "B1",
                ListB = new List<B>{ new B { Name = "BB1"} }
            },new A{
                B = new B{Name="2"},
                Name = "B2",
                ListB = new List<B>{ new B { Name = "BB2"} }
            },
            };

            var test = list.AsQueryable().Select<A>(new ParsingConfig()
            {
                AllowNewToEvaluateAnyType = true,
                ResolveTypesBySimpleName = true
            },
            "new (Name,new B(B.Name) as B,ListB.Select(new B(Name)).ToList() as ListB)").ToList();
            foreach (var t in test)
            {
                Console.WriteLine($"{t.Name},{t.B.Name},{t.ListB.FirstOrDefault()?.Name}");
            }
        }
    }
}
