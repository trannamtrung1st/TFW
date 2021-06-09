using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TFW.Framework.CQRSExamples.Models.Command
{
    public class DeleteProductCommand : IRequest
    {
        public string Id { get; set; }
    }
}
