using ERP.Domain.Dtos;
using ERP.Domain.Entities;
using ERP.Domain.Repositories;
using GenericRepository;
using MediatR;
using TS.Result;

namespace ERP.Application.Features.Recipies.Create
{
    public sealed record RecipeCreateCommand(
        Guid ProductId,
        List<RecipeDetailDto> Details
        )
        : IRequest<Result<string>>;

    internal sealed class RecipeCreateCommandHandler(
        IRecipeRepository recipeRepository,
        IUnitOfWork unitOfWork)
        : IRequestHandler<RecipeCreateCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(RecipeCreateCommand request, CancellationToken cancellationToken)
        {

            bool isRecipeExists = await recipeRepository
                .AnyAsync(p => p.ProductId == request.ProductId, cancellationToken);

            if(isRecipeExists)
            {
                return Result<string>.Failure(409, "Recipe already exists.");
            }

            Recipe recipe = new()
            {
                ProductId = request.ProductId,
                Details = request.Details.Select(s => new RecipeDetail()
                {
                    ProductId = s.ProductId,
                    Quantity = s.Quantity
                })
                .ToList()
            };

            await recipeRepository.AddAsync(recipe);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<string>.Succeed("Recipe successfully created.");

        }
    }
}
