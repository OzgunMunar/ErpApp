using ERP.Domain.Entities;
using ERP.Domain.Enums;
using ERP.Domain.Repositories;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ERP.Application.Features.Orders.GetAll
{
    public sealed record OrdersGetAllQuery()
        : IRequest<IQueryable<OrdersGetAllQueryResponse>>;

    public sealed record OrdersGetAllQueryResponse(

        Guid Id,
        Guid CustomerId,
        Customer? Customer,
        string OrderNumber,
        DateOnly OrderedDate,
        DateOnly DeliveryDate,
        int Status,
        List<OrderDetail>? Details
        
    );

    internal sealed class OrdersGetAllQueryHandler(
        IOrderRepository orderRepository)
        : IRequestHandler<OrdersGetAllQuery, IQueryable<OrdersGetAllQueryResponse>>
    {
        public async Task<IQueryable<OrdersGetAllQueryResponse>> Handle(OrdersGetAllQuery request, CancellationToken cancellationToken)
        {

            var orders = await orderRepository
                .GetAll()
                .Where(order => order.IsDeleted == false)
                .Include(c => c.Customer)
                .Include(p => p.Details!.Where(d => d.IsDeleted == false))
                .ThenInclude(p => p.Product)
                .OrderByDescending(v => v.OrderedDate)
                .Select(ord => new OrdersGetAllQueryResponse
                (
                    ord.Id,
                    ord.CustomerId,
                    ord.Customer,
                    ord.OrderNumber,
                    ord.OrderedDate,
                    ord.DeliveryDate,
                    ord.Status,
                    ord.Details == null
                        ? new List<OrderDetail>()
                        : ord.Details
                            .Where(d => !d.IsDeleted)
                            .Select(d => new OrderDetail
                            {
                                Id = d.Id,
                                OrderId = d.OrderId,
                                ProductId = d.ProductId,
                                Quantity = d.Quantity,
                                Price = d.Price,
                                Product = d.Product
                            })
                            .ToList()

                ))
                .ToListAsync(cancellationToken);

            return orders.AsQueryable();

        }
    }
}
