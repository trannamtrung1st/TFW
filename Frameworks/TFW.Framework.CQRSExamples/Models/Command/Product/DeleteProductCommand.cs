using MediatR;

namespace TFW.Framework.CQRSExamples.Models.Command
{
    public class DeleteProductCommand : IRequest
    {
        public string Id { get; set; }
    }
}
