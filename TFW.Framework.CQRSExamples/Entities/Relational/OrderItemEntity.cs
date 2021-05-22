namespace TFW.Framework.CQRSExamples.Entities.Relational
{
    public class OrderItemEntity
    {
        public string ProductId { get; set; }
        public string OrderId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public virtual ProductEntity Product { get; set; }
        public virtual OrderEntity Order { get; set; }
    }
}
