using ERP.Domain.Entities;
using ERP.Domain.Repositories;
using GenericRepository;
using MediatR;
using TS.Result;

namespace ERP.Application.Features.Recipies.Delete
{
    public sealed record RecipeDeleteCommand(Guid Id)
        : IRequest<Result<string>>;

    internal sealed class RecipeDeleteCommandHandler(
        IRecipeRepository recipeRepository,
        IUnitOfWork unitOfWork)
        : IRequestHandler<RecipeDeleteCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(RecipeDeleteCommand request, CancellationToken cancellationToken)
        {

            Recipe? recipe = await recipeRepository
                .FirstOrDefaultAsync(rec => rec.Id == request.Id, cancellationToken);

            if (recipe == null)
            {
                return Result<string>.Failure(404, "Recipe not found");
            }

            recipe.IsDeleted = true;

            recipeRepository.Update(recipe);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<string>.Succeed("Recipe successfully deleted.");

        }
    }
}