namespace TFW.Framework.CQRSExamples.Entities.Relational
{
    public class ProductEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CategoryId { get; set; }
        public decimal UnitPrice { get; set; }

        public virtual ProductCategoryEntity Category { get; set; }
    }
}
