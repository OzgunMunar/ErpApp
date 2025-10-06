using ERP.Domain.Dtos;
using ERP.Domain.Entities;
using ERP.Domain.Repositories;
using GenericRepository;
using MapsterMapper;
using MediatR;
using TS.Result;

namespace ERP.Application.Features.Orders.Create
{
    public sealed record OrderCreateCommand
        (
            Guid CustomerId,
            DateTime OrderedDate,
            DateTime DeliveryDate,
            List<OrderDetailDto> Details
        )
        : IRequest<Result<string>>;

    internal sealed class OrderCreateCommandHandler(
        IOrderRepository orderRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
        : IRequestHandler<OrderCreateCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(OrderCreateCommand request, CancellationToken cancellationToken)
        {

            //List<OrderDetail> details = request.Details
            //    .Select(detail => new OrderDetail
            //    {
            //        Price = detail.Price,
            //        ProductId = detail.ProductId,
            //        Quantity = detail.Quantity,
            //    }).ToList();

            //Order order = new()
            //{
            //    CustomerId = request.CustomerId,
            //    OrderedDate = request.OrderedDate,
            //    DeliveryDate = request.DeliveryDate,

            //    Details = details
            //};

            Order order = mapper.Map<Order>(request);
            order.OrderNumber = $"ERP-{DateTime.Now.Year}-{Guid.NewGuid().ToString().Split('-').Last().ToUpper()}";

            await orderRepository.AddAsync(order, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<string>.Succeed("Order successfully completed.");

        }
    }

}
