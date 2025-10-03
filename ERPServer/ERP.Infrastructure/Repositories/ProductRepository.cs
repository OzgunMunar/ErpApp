using ERP.Domain.Entities;
using ERP.Domain.Repositories;
using ERP.Infrastructure.Context;
using GenericRepository;

namespace ERP.Infrastructure.Repositories
{
    public sealed class ProductRepository(ApplicationDbContext context) : Repository<Product, ApplicationDbContext>(context), IProductRepository
    {
    }
}
