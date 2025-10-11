using ERP.Domain.Entities;
using ERP.Domain.Repositories;
using ERP.Infrastructure.Context;
using GenericRepository;

namespace ERP.Infrastructure.Repositories
{
    public sealed class InvoiceDetailRepository(ApplicationDbContext context) : Repository<InvoiceDetail, ApplicationDbContext>(context), IInvoiceDetailRepository
    {
    }
}
