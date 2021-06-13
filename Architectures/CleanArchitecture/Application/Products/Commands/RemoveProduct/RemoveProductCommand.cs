using Domain.Products;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Products.Commands.RemoveProduct
{
    public class RemoveProductCommand : IRequest<Product>
    {
        public int ProductId { get; set; }
    }
}
