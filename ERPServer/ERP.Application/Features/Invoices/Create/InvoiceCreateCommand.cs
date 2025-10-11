using ERP.Application.Features.Orders.Create;
using ERP.Domain.Dtos;
using ERP.Domain.Entities;
using ERP.Domain.Enums;
using ERP.Domain.Repositories;
using GenericRepository;
using Mapster;
using MapsterMapper;
using MediatR;
using TS.Result;

namespace ERP.Application.Features.Invoices.Create
{
    public sealed record InvoiceCreateCommand(
            Guid CustomerId,
            DateOnly Date,
            string InvoiceNumber,
            List<InvoiceDetailDto> Details,
            int InvoiceType

    ) : IRequest<Result<string>>;

    internal sealed record InvoiceCreateCommandHandler(
        
        IInvoiceRepository invoiceRepository,
        IUnitOfWork unitOfWork,
        IStockMovementRepository stockMovementRepository,
        IMapper mapper
        
        )
        : IRequestHandler<InvoiceCreateCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(InvoiceCreateCommand request, CancellationToken cancellationToken)
        {

            var config = new TypeAdapterConfig();
            config.NewConfig<InvoiceCreateCommand, Invoice>()
                .Map(dest => dest.InvoiceType, src => InvoiceTypeEnum.FromValue(src.InvoiceType));

            Invoice invoice = mapper.Adapt<Invoice>(config);
            invoice.InvoiceNumber = $"ERP-{DateTime.Now.Year}-{Guid.NewGuid().ToString().Split('-').Last().ToUpper()}";

            if(invoice.Details is not null)
            {

                List<StockMovement> movements = new(); 

                foreach (var item in invoice.Details)
                {
                    StockMovement movement = new()
                    {
                        InvoiceId = item.InvoiceId,
                        NumberOfEntry = request.InvoiceType == 1 ? item.Quantity : 0,
                        NumberOfOutputs = request.InvoiceType == 2 ? item.Quantity : 0,
                        DepotId = item.DepotId,
                        Price = item.Price,
                        ProductId = item.ProductId
                    };

                    movements.Add(movement);

                }

                await stockMovementRepository.AddRangeAsync(movements);

            }

            await invoiceRepository.AddAsync(invoice, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<string>.Succeed("Invoice successfully created.");

        }
    }
}
