using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TFW.Framework.CQRSExamples.Models.Command
{
    public class CreateProductCategoryCommand : IRequest<string>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
