using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TFW.Framework.DI.Examples.Loggers;
using TFW.Framework.DI.Examples.Models;

namespace TFW.Framework.DI.Examples
{
    public static class Overview
    {
        public static class NormalWay
        {
            public static void Run()
            {
                var productService = new ProductService();

                productService.AddProduct(new Product("Product A"));
            }

#if false
            public class ProductService
            {
                private ConsoleLogger _logger;

                public ProductService()
                {
                    _logger = new ConsoleLogger();
                }

                public void AddProduct(Product product)
                {
                    // save to database

                    _logger.LogToConsole($"Added new product at {DateTime.Now}");
                }
            }
#else
            public class ProductService
            {
                private FileLogger _logger;

                public ProductService()
                {
                    _logger = new FileLogger("./logs/log.txt");
                }

                public void AddProduct(Product product)
                {
                    // save to database

                    _logger.LogToFile($"Added new product at {DateTime.Now}");
                }
            }
#endif
        }

        public static class DevAndComputer
        {
            public class Dev
            {
                private IComputer _computer;

                public Dev(IComputer computer)
                {
                    _computer = computer;
                }

                public void Work()
                {
                    _computer.Start();

                    // do the work
                }
            }

            public interface IComputer
            {
                void Start();
            }

            public class Computer : IComputer
            {
                public Computer(string model, string cpu, string ram)
                {
                    Model = model;
                    CPU = cpu;
                    RAM = ram;
                }

                public string Model { get; set; }
                public string CPU { get; set; }
                public string RAM { get; set; }

                public void Start()
                {
                    // start the computer
                }
            }

            public static void Run()
            {
                Console.Write("Input computer model: ");
                var computerModel = Console.ReadLine();

                Console.Write("Input computer CPU: ");
                var computerCPU = Console.ReadLine();

                Console.Write("Input computer RAM: ");
                var computerRAM = Console.ReadLine();

                var computer = new Computer(computerModel, computerCPU, computerRAM);

                var dev = new Dev(computer);
                dev.Work();
            }
        }

        public static class WorkingWithDb
        {
            public static void Run()
            {
                var productService = new ProductService();
                var orderService = new OrderService();

                // 2 db connections are created
                var products = productService.GetProducts();
                var orders = orderService.GetOrders();

                Console.WriteLine(products.Count());
                Console.WriteLine(orders.Count());
            }

            public class ProductService
            {
                private SqlConnection _connection;

                public ProductService()
                {
                    _connection = new SqlConnection();
                }

                public IEnumerable<Product> GetProducts()
                {
                    // query the products
                    var list = _connection.Query<Product>("{SQL query}");

                    return list;
                }
            }

            public class OrderService
            {
                private SqlConnection _connection;

                public OrderService()
                {
                    _connection = new SqlConnection();
                }

                public IEnumerable<Order> GetOrders()
                {
                    // query the orders
                    var list = _connection.Query<Order>("{SQL query}");

                    return list;
                }
            }

            public class SqlConnection : IDisposable
            {
                public void Dispose()
                {
                }

                public IEnumerable<T> Query<T>(string queryStr)
                {
                    return default;
                }
            }
        }
    }

    public static class DIWay
    {
        public class ProductService
        {
            private ILogger _logger;

            public ProductService(ILogger logger)
            {
                _logger = logger;
            }

            public void AddProduct(Product product)
            {
                // save to database

                _logger.Log($"Added new product at {DateTime.Now}");
            }
        }

        public static void Run()
        {
            ILogger logger;

            if (IsDevelopment())
            {
                logger = new ConsoleLogger();
            }
            else
            {
                logger = new FileLogger("logs/log.txt");
            }

            var productService = new ProductService(logger);
            productService.AddProduct(new Product("Product A"));
        }

        public static bool IsDevelopment()
        {
            return true;
        }

    }
}
