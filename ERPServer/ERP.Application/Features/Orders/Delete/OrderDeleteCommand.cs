using ERP.Domain.Entities;
using ERP.Domain.Repositories;
using GenericRepository;
using MediatR;
using TS.Result;

namespace ERP.Application.Features.Orders.Delete
{
    public sealed record OrderDeleteCommand(Guid Id)
        : IRequest<Result<string>>;

    internal sealed class OrderDeleteCommandHandler(
        IOrderRepository orderRepository,
        IUnitOfWork unitOfWork)
        : IRequestHandler<OrderDeleteCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(OrderDeleteCommand request, CancellationToken cancellationToken)
        {
            
            Order? order = await orderRepository
                .FirstOrDefaultAsync(or => 
                or.Id == request.Id,
                cancellationToken);

            if (order is null)
            {
                return Result<string>.Failure(404, "Order not found.");
            }

            order.IsDeleted = true;

            orderRepository.Update(order);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<string>.Succeed("Order successfully deleted.");

        }
    }

}
