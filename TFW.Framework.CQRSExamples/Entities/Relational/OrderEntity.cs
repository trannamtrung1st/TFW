using System;
using System.Collections.Generic;

namespace TFW.Framework.CQRSExamples.Entities.Relational
{
    public class OrderEntity
    {
        public string Id { get; set; }
        public string SessionId { get; set; }
        public DateTimeOffset Time { get; set; }

        public virtual SessionEntity Session { get; set; }
        public virtual ICollection<OrderItemEntity> OrderItems { get; set; }
    }
}
