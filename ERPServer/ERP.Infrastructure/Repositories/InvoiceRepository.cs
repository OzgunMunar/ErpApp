using ERP.Domain.Entities;
using ERP.Domain.Repositories;
using ERP.Infrastructure.Context;
using GenericRepository;

namespace ERP.Infrastructure.Repositories
{
    public sealed class InvoiceRepository(ApplicationDbContext context) : Repository<Invoice, ApplicationDbContext>(context), IInvoiceRepository
    {
    }
}
