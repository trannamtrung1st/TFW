using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TFW.Framework.CQRSExamples.Entities.Relational;
using TFW.Framework.CQRSExamples.Models.Notification;

namespace TFW.Framework.CQRSExamples.Models.Command
{
    public class OrderCommandHandler : IRequestHandler<CreateOrderCommand, string>
    {
        private readonly RelationalContext _relationalContext;
        private readonly IMediator _mediator;

        public OrderCommandHandler(
            RelationalContext relationalContext,
            IMediator mediator)
        {
            _relationalContext = relationalContext;
            _mediator = mediator;
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

            using (var trans = await _relationalContext.Database.BeginTransactionAsync())
            {
                _relationalContext.Add(entity);

                await _relationalContext.SaveChangesAsync();

                await trans.CommitAsync();

                await _mediator.Publish(new CreateOrderEvent
                {
                    Address = entity.Address,
                    Customer = new CreateOrderEvent.CustomerModel
                    {
                        CreatedTime = customer.CreatedTime,
                        Id = customer.Id,
                        Name = customer.Name
                    },
                    CustomerId = customer.Id,
                    Id = entity.Id,
                    Phone = entity.Phone,
                    Time = entity.Time,
                    OrderItems = entity.OrderItems.Select(o => new CreateOrderEvent.OrderItem
                    {
                        OrderId = o.OrderId,
                        ProductId = o.ProductId,
                        Quantity = o.Quantity,
                        UnitPrice = o.UnitPrice
                    }).ToArray()
                });
            }

            return entity.Id;
        }
    }
}
