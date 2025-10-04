using ERP.Domain.Entities;
using ERP.Domain.Repositories;
using ERP.Infrastructure.Context;
using GenericRepository;

namespace ERP.Infrastructure.Repositories
{
    public sealed class RecipeDetailRepository(ApplicationDbContext context) : Repository<RecipeDetail, ApplicationDbContext>(context), IRecipeDetailsRepository
    {
    }
}
