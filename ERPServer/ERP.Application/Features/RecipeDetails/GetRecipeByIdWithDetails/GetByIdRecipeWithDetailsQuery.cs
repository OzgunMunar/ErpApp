using ERP.Domain.Entities;
using ERP.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace ERP.Application.Features.RecipeDetails.GetRecipeByIdWithDetails
{
    public sealed record GetRecipeByIdWithDetailsQuery(Guid Id)
        :IRequest<Result<Recipe>>;

    internal sealed class GetRecipeByIdWithDetailsQueryHandler(
        IRecipeRepository recipeRepository)
        : IRequestHandler<GetRecipeByIdWithDetailsQuery, Result<Recipe>>
    {
        public async Task<Result<Recipe>> Handle(GetRecipeByIdWithDetailsQuery request, CancellationToken cancellationToken)
        {

            Recipe? recipe = await recipeRepository
                .Where(p => p.Id == request.Id && !p.IsDeleted)
                .Include(p => p.Product)
                .Include(p => p.Details!.Where(d => !d.IsDeleted))
                    .ThenInclude(d => d.Product)
                .FirstOrDefaultAsync(cancellationToken);

            if (recipe is null)
            {
                return Result<Recipe>.Failure(404, "Recipe not found");
            }

            return recipe;

        }
    }


}
