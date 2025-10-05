using ERP.Domain.Entities;
using ERP.Domain.Repositories;
using ERP.Infrastructure.Context;
using GenericRepository;

namespace ERP.Infrastructure.Repositories
{
    public sealed class DepotRepository(ApplicationDbContext context) : Repository<Depot, ApplicationDbContext>(context), IDepotRepository
    {
    }
}
