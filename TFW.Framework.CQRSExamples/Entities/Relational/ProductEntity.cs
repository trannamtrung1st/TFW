namespace TFW.Framework.CQRSExamples.Entities.Relational
{
    public class ProductEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double UnitPrice { get; set; }
        public string CategoryId { get; set; }
        public string StoreId { get; set; }
        public string BrandId { get; set; }

        public virtual ProductCategoryEntity Category { get; set; }
        public virtual StoreEntity Store { get; set; }
        public virtual BrandEntity Brand { get; set; }
    }
}
