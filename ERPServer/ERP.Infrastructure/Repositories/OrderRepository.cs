using ERP.Domain.Entities;
using ERP.Domain.Repositories;
using ERP.Infrastructure.Context;
using GenericRepository;

namespace ERP.Infrastructure.Repositories
{
    public class OrderRepository(ApplicationDbContext context) : Repository<Order, ApplicationDbContext>(context), IOrderRepository
    {
    }
}
