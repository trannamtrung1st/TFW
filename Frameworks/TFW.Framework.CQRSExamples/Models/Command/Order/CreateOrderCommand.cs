using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TFW.Framework.CQRSExamples.Models.Command
{
    public class CreateOrderCommand : IRequest<string>
    {
        public string CustomerName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }

        public class OrderItem
        {
            public string ProductId { get; set; }
            public int Quantity { get; set; }
            public double UnitPrice { get; set; }
        }
    }
}
