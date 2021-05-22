using System;

namespace TFW.Framework.CQRSExamples.Entities.Relational
{
    public class CustomerEntity
    {
        public CustomerEntity()
        {
            CreatedTime = DateTimeOffset.UtcNow;
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
    }
}
