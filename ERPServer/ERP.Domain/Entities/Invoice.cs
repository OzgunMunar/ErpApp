using ERP.Domain.Abstractions;
using ERP.Domain.Entities;
using ERP.Domain.Enums;

namespace ERP.Domain.Entities
{
    public sealed class Invoice : Entity
    {
        public Guid CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public string InvoiceNumber { get; set; } = default!;
        public DateOnly InvoiceDate { get; set; }
        public InvoiceTypeEnum InvoiceType { get; set; } = InvoiceTypeEnum.Purchase;
        public List<InvoiceDetail>? Details { get; set; }
    }
}