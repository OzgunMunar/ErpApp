using ERP.Domain.Entities;
using ERP.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ERP.Application.Features.Invoices.GetAll
{
    public sealed record InvoiceGetAllQuery(
        
            int InvoiceType
        
        )
        : IRequest<IQueryable<InvoiceGetAllQueryResponse>>;

    public sealed record InvoiceGetAllQueryResponse(
        
            Guid Id,
            Guid CustomerId,
            Customer? Customer,
            string InvoiceNumber,
            DateOnly InvoiceDate,
            int InvoiceType,
            List<InvoiceDetail>? Details
        
        );

    internal sealed class InvoiceGetAllQueryHandler(
        
            IInvoiceRepository invoiceRepository
        
        )
        : IRequestHandler<InvoiceGetAllQuery, IQueryable<InvoiceGetAllQueryResponse>>
    {
        public async Task<IQueryable<InvoiceGetAllQueryResponse>> Handle(InvoiceGetAllQuery request, CancellationToken cancellationToken)
        {

            var invoices = await invoiceRepository
                .Where(i => 
                    i.InvoiceType.Value == request.InvoiceType
                    &&
                    i.IsDeleted == false)
                .OrderBy(i => i.InvoiceDate)
                .Select(i => new InvoiceGetAllQueryResponse
                (
                    i.Id,
                    i.CustomerId,
                    i.Customer,
                    i.InvoiceNumber,
                    i.InvoiceDate,
                    i.InvoiceType,
                    i.Details == null ?
                        new List<InvoiceDetail>()
                        : i.Details
                            .Where(d => !d.IsDeleted)
                            .Select(d => new InvoiceDetail
                            {
                                Id = d.Id,
                                InvoiceId = d.InvoiceId,
                                ProductId = d.ProductId,
                                Product = d.Product,
                                Quantity = d.Quantity,
                                Price = d.Price
                            })
                            .ToList()

                ))
                .ToListAsync(cancellationToken);

            return invoices.AsQueryable();

        }
    }
}