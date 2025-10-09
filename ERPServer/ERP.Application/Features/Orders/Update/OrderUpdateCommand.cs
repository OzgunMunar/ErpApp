using ERP.Domain.Dtos;
using ERP.Domain.Entities;
using ERP.Domain.Enums;
using ERP.Domain.Repositories;
using GenericRepository;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace ERP.Application.Features.Orders.Update
{

    public sealed record OrderUpdateCommand(

        Guid Id,
        Guid CustomerId,
        DateOnly OrderedDate,
        DateOnly DeliveryDate,
        List<OrderDetailDto> Details,
        int Status
    )
    : IRequest<Result<string>>;

    internal sealed class UpdateOrderCommandHandler(
        IOrderRepository orderRepository,
        IOrderDetailRepository orderDetailRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
        : IRequestHandler<OrderUpdateCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(OrderUpdateCommand request, CancellationToken cancellationToken)
        {

            Order? order = await orderRepository
                .WhereWithTracking(or =>
                    or.Id == request.Id
                    &&
                    or.IsDeleted == false)
                .Include(d => d.Details)
                .FirstOrDefaultAsync(cancellationToken);

            if (order is null)
            {
                return Result<string>.Failure(404, "Order not found.");
            }

            foreach (var existingDetail in order.Details!)
            {
                var incomingDetail = request.Details.FirstOrDefault(d => d.ProductId == existingDetail.ProductId);

                if (incomingDetail is not null)
                {
                    existingDetail.Quantity = incomingDetail.Quantity;
                    existingDetail.Price = incomingDetail.Price;
                    existingDetail.IsDeleted = false;
                }
                else
                {
                    existingDetail.IsDeleted = true;
                }
            }

            var existingProductIds = order.Details.Select(d => d.ProductId).ToHashSet();

            var newDetails = request.Details
                .Where(d => !existingProductIds.Contains(d.ProductId))
                .Select(d => new OrderDetail
                {
                    Id = Guid.NewGuid(),
                    OrderId = order.Id,
                    ProductId = d.ProductId,
                    Quantity = d.Quantity,
                    Price = d.Price,
                    IsDeleted = false
                })
                .ToList();

            order.Details.AddRange(newDetails);

            order.CustomerId = request.CustomerId;
            order.OrderedDate = request.OrderedDate;
            order.DeliveryDate = request.DeliveryDate;
            order.Status = OrderStatusEnum.FromValue(request.Status);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<string>.Succeed("Order successfully updated");

        }

    }

}