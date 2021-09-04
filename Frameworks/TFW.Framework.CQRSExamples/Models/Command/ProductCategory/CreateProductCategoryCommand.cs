using MediatR;

namespace TFW.Framework.CQRSExamples.Models.Command
{
    public class CreateProductCategoryCommand : IRequest<string>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
