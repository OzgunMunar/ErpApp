using ERP.Domain.Entities;
using ERP.Domain.Repositories;
using GenericRepository;
using MediatR;
using TS.Result;

namespace ERP.Application.Features.RecipeDetails.Delete
{
    public sealed record RecipeDetailDeleteCommand(Guid Id)
        : IRequest<Result<string>>;

    internal sealed class RecipeDetailDeleteCommandHandler(
        IRecipeDetailsRepository recipeDetailsRepository,
        IUnitOfWork unitOfWork)
        : IRequestHandler<RecipeDetailDeleteCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(RecipeDetailDeleteCommand request, CancellationToken cancellationToken)
        {

            RecipeDetail recipeDetail = await recipeDetailsRepository
                .GetByExpressionAsync(p => p.Id == request.Id, cancellationToken);

            if (recipeDetail == null)
            {
                return Result<string>.Failure(404, "Recipe not found.");
            }

            recipeDetail.IsDeleted = true;

            recipeDetailsRepository.Update(recipeDetail);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<string>.Succeed("Recipe is saved.");

        }

    }

}
