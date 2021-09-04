using System.Collections.Generic;

namespace TFW.Framework.CQRSExamples.Entities.Relational
{
    public class BrandEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<ProductEntity> Products { get; set; }
    }
}
