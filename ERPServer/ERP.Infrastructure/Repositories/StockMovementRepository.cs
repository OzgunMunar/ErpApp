using ERP.Domain.Entities;
using ERP.Domain.Repositories;
using ERP.Infrastructure.Context;
using GenericRepository;

namespace ERP.Infrastructure.Repositories
{
    internal sealed class StockMovementRepository(ApplicationDbContext context) : Repository<StockMovement, ApplicationDbContext>(context), IStockMovementRepository
    {
    }
}
