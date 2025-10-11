using ERP.Application.Features.Invoices.Create;
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

namespace ERP.Application.Features.Invoices.Update
{
    public sealed record InvoiceUpdateCommand(
        
        Guid Id,
        Guid CustomerId,
        int InvoiceType,
        DateOnly InvoiceDate,
        string InvoiceNumber,
        List<InvoiceDetailDto> Details
        
        )
        : IRequest<Result<string>>;

    internal sealed class InvoiceUpdateCommandHandler(
        
        IInvoiceRepository invoiceRepository,
        IStockMovementRepository stockMovementRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper 
        
        )
        : IRequestHandler<InvoiceUpdateCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(InvoiceUpdateCommand request, CancellationToken cancellationToken)
        {

            Invoice? invoice = await invoiceRepository
                .GetByExpressionWithTrackingAsync(i => i.Id == request.Id, cancellationToken);

            if (invoice is null)
            {
                return Result<string>.Failure(404, "Invoice not found");
            }

            List<StockMovement> movements = await stockMovementRepository
                .Where(
                    p => p.InvoiceId == invoice.Id
                    &&
                    p.IsDeleted == false)
                .ToListAsync(cancellationToken);

            stockMovementRepository.DeleteRange(movements);

            mapper.Map(request, invoice);

            List<StockMovement> newMovements = new();
            foreach (var item in request.Details)
            {

                StockMovement movement = new()
                {
                    InvoiceId = invoice.Id,
                    NumberOfEntry = request.InvoiceType == 1 ? item.Quantity : 0,
                    NumberOfOutputs = request.InvoiceType == 2 ? item.Quantity : 0,
                    DepotId = item.DepotId,
                    Price = item.Price,
                    ProductId = item.ProductId
                };

                newMovements.Add(movement);

            }

            await stockMovementRepository.AddRangeAsync(newMovements, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<string>.Succeed("Invoice successfully created.");

        }
    }
}
