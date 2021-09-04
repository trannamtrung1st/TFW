namespace TFW.Framework.DI.Examples.Models
{
    public class Order
    {
        public string Id { get; set; }

        public Order(string id)
        {
            Id = id;
        }
    }
}
