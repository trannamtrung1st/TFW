using System;
using System.Collections.Generic;

namespace TFW.Framework.CQRSExamples.Entities.Relational
{
    public class OrderEntity
    {
        public OrderEntity()
        {
            Time = DateTime.UtcNow;
        }

        public string Id { get; set; }
        public string CustomerId { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime Time { get; set; }

        public virtual CustomerEntity Customer { get; set; }
        public virtual ICollection<OrderItemEntity> OrderItems { get; set; }
    }
}
