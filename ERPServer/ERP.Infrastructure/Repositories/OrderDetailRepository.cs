using ERP.Domain.Entities;
using ERP.Domain.Repositories;
using ERP.Infrastructure.Context;
using GenericRepository;

namespace ERP.Infrastructure.Repositories
{
    internal class OrderDetailRepository(ApplicationDbContext context) : Repository<OrderDetail, ApplicationDbContext>(context), IOrderDetailRepository
    {
    }
}
