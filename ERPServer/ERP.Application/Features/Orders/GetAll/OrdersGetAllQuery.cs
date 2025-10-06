using ERP.Domain.Entities;
using ERP.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ERP.Application.Features.Orders.GetAll
{
    public sealed record OrdersGetAllQuery()
        : IRequest<IQueryable<Order>>;

    internal sealed class OrdersGetAllQueryHandler(
        IOrderRepository orderRepository)
        : IRequestHandler<OrdersGetAllQuery, IQueryable<Order>>
    {
        public async Task<IQueryable<Order>> Handle(OrdersGetAllQuery request, CancellationToken cancellationToken)
        {

            List<Order> orders = await orderRepository
                .GetAll()
                .Where(order => order.IsDeleted == false)
                .Include(c => c.Customer)
                .Include(p => p.Details!.Where(d => d.IsDeleted == false))
                .ThenInclude(p => p.Product)
                .OrderByDescending(v => v.OrderedDate)
                .ToListAsync(cancellationToken);

            return orders.AsQueryable();

        }
    }
}
