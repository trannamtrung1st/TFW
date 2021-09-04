using MediatR;

namespace TFW.Framework.CQRSExamples.Models.Command
{
    public class UpdateProductCategoryCommand : IRequest
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
