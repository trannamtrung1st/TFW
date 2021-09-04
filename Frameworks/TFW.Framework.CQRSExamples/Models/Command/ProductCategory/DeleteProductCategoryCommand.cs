using MediatR;

namespace TFW.Framework.CQRSExamples.Models.Command
{
    public class DeleteProductCategoryCommand : IRequest
    {
        public string Id { get; set; }
    }
}
