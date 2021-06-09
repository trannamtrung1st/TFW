using System;

namespace TFW.Framework.CQRSExamples.Entities.Relational
{
    public class CustomerEntity
    {
        public CustomerEntity()
        {
            CreatedTime = DateTime.UtcNow;
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
