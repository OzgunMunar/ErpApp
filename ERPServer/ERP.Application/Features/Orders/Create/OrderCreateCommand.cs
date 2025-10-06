using ERP.Application.Features.Products.Create;
using ERP.Domain.Dtos;
using ERP.Domain.Entities;
using ERP.Domain.Enums;
using ERP.Domain.Repositories;
using GenericRepository;
using Mapster;
using MapsterMapper;
using MediatR;
using TS.Result;

namespace ERP.Application.Features.Orders.Create
{
    public sealed record OrderCreateCommand
        (
            Guid CustomerId,
            DateOnly OrderedDate,
            DateOnly DeliveryDate,
            List<OrderDetailDto> Details,
            int Status
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

            var config = new TypeAdapterConfig();
            config.NewConfig<OrderCreateCommand, Order>()
                .Map(dest => dest.Status, src => OrderStatusEnum.FromValue(src.Status));

            Order order = mapper.Adapt<Order>(config);
            order.OrderNumber = $"ERP-{DateTime.Now.Year}-{Guid.NewGuid().ToString().Split('-').Last().ToUpper()}";

            await orderRepository.AddAsync(order, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<string>.Succeed("Order successfully completed.");

        }
    }

}
