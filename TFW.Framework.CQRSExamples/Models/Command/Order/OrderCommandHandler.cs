using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TFW.Framework.CQRSExamples.Entities.Relational;

namespace TFW.Framework.CQRSExamples.Models.Command
{
    public class OrderCommandHandler : IRequestHandler<CreateOrderCommand, string>
    {
        private readonly RelationalContext _relationalContext;

        public OrderCommandHandler(
            RelationalContext relationalContext)
        {
            _relationalContext = relationalContext;
        }

        public async Task<string> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var customer = await _relationalContext.Customers.FirstOrDefaultAsync(o => o.Name == request.CustomerName);

            if (customer == null)
                customer = new CustomerEntity
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = request.CustomerName
                };

            var entity = new OrderEntity
            {
                Id = Guid.NewGuid().ToString(),
                Address = request.Address,
                Phone = request.Phone,
                Customer = customer,
                OrderItems = request.OrderItems.Select(o => new OrderItemEntity
                {
                    ProductId = o.ProductId,
                    Quantity = o.Quantity,
                    UnitPrice = o.UnitPrice
                }).ToArray()
            };

            _relationalContext.Add(entity);

            await _relationalContext.SaveChangesAsync();

            // [TODO] notify

            return entity.Id;
        }
    }
}
