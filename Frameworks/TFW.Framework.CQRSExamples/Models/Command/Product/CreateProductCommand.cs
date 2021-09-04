using MediatR;

namespace TFW.Framework.CQRSExamples.Models.Command
{
    public class CreateProductCommand : IRequest<string>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string CategoryId { get; set; }
        public string StoreId { get; set; }
        public string BrandId { get; set; }
        public double UnitPrice { get; set; }
    }
}
