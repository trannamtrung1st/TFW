using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using TFW.Framework.Common.Helpers;
using TFW.Framework.ConsoleApp;
using TFW.Framework.i18n.Helpers;

namespace TFW
{
    public class TestConsoleApp : DefaultConsoleTask
    {
        public IDictionary<string, Func<Task>> Tasks => new Dictionary<string, Func<Task>>()
        {
        };

        public override string Title => "Test task";

        public override string Description => "Test task description";

        public override Task StartAsync()
        {
            return Task.CompletedTask;
        }
    }

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
            //var list = new List<A> { new A
            //{
            //    B = new B{Name="1"},
            //    Name = "B1",
            //    ListB = new List<B>{ new B { Name = "BB1"} }
            //},new A{
            //    B = new B{Name="2"},
            //    Name = "B2",
            //    ListB = new List<B>{ new B { Name = "BB2"} }
            //},
            //};

            //var test = list.AsQueryable().Select<A>(new ParsingConfig()
            //{
            //    AllowNewToEvaluateAnyType = true,
            //    ResolveTypesBySimpleName = true
            //},
            //"new (Name,new B(B.Name) as B,ListB.Select(new B(Name)).ToList() as ListB)").ToList();
            //foreach (var t in test)
            //{
            //    Console.WriteLine($"{t.Name},{t.B.Name},{t.ListB.FirstOrDefault()?.Name}");
            //}

            //Console.WriteLine(TimeZoneHelper.GetFirstTimeZoneByUTCOffset(TimeSpan.FromMinutes(420)));

            //Console.WriteLine(typeof(List<>).GetNameWithoutGenericParameters());
            //Console.WriteLine(typeof(List<string>).GetNameWithoutGenericParameters());

            //foreach (var region in CultureInfo.GetCultures(CultureTypes.SpecificCultures)
            //    .Select(o => o.LCID).Distinct().Select(o => new RegionInfo(o)))
            //{
            //    Console.WriteLine(region.Name + " - " + region.CurrencySymbol + " - " + region.ISOCurrencySymbol);
            //}

            var fileInfo = new FileInfo(@"C:\Users\trann\Desktop\test\test.txt");
            fileInfo.MoveTo(@"C:\Users\trann\Desktop\test\2.txt", true);
        }
    }
}
