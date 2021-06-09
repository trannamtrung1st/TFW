using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TFW.Framework.CQRSExamples.Entities.Relational
{
    public class StoreEntity
    {
        public string Id { get; set; }
        public string StoreName { get; set; }
        public string Address { get; set; }

        public virtual ICollection<ProductEntity> Products { get; set; }
    }
}
