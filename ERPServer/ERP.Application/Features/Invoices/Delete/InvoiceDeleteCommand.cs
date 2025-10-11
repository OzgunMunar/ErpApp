using ERP.Domain.Entities;
using ERP.Domain.Repositories;
using GenericRepository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.Result;

namespace ERP.Application.Features.Invoices.Delete
{
    public sealed record InvoiceDeleteCommand(Guid Id)
        : IRequest<Result<string>>;

    internal sealed class InvoiceDeleteCommandHandler(
        
        IInvoiceRepository invoiceRepository,
        IStockMovementRepository stockMovementRepository,
        IUnitOfWork unitOfWork

        )
        : IRequestHandler<InvoiceDeleteCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(InvoiceDeleteCommand request, CancellationToken cancellationToken)
        {

            Invoice? invoice = await invoiceRepository
                .GetByExpressionAsync(i => i.Id == request.Id, cancellationToken);

            if(invoice is null)
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

            invoiceRepository.Delete(invoice);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<string>.Succeed("Invoice successfully deleted.");

        }
    }
}
