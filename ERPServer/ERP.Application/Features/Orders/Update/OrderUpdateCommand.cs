using ERP.Domain.Dtos;
using ERP.Domain.Entities;
using ERP.Domain.Repositories;
using GenericRepository;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace ERP.Application.Features.Orders.Update
{
    public sealed record OrderUpdateCommand(
        
        Guid Id,
        Guid CustomerId,
        DateTime OrderedDate,
        DateTime DeliveryDate,
        List<OrderDetailDto> Details
        )
        : IRequest<Result<string>>;

    internal sealed class UpdateOrderCommandHandler(
        IOrderRepository orderRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
        : IRequestHandler<OrderUpdateCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(OrderUpdateCommand request, CancellationToken cancellationToken)
        {

            Order? order = await orderRepository
                .Where(or =>
                    or.Id == request.Id)
                .Include(d => d.Details)
                .FirstOrDefaultAsync(cancellationToken);

            if (order is null)
            {
                return Result<string>.Failure(404, "Order not found.");
            }

            foreach (var item in order.Details!)
            {
                item.IsDeleted = true;
            }

            mapper.Map(request, order);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<string>.Succeed("Order successfully updated");

        }

    }

}
