using Domain.Sales;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Sales.Commands.CreateSale
{
    public class CreateSaleCommand : IRequest<Sale>
    {
        public int CustomerId { get; set; }

        public int EmployeeId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }
    }
}
