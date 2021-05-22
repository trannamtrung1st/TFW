using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TFW.Framework.CQRSExamples.Models.Command
{
    public class UpdateProductCommand : IRequest
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CategoryId { get; set; }
        public string StoreId { get; set; }
        public string BrandId { get; set; }
        public double UnitPrice { get; set; }
    }
}
