using ERP.Domain.Entities;
using ERP.Domain.Repositories;
using GenericRepository;
using MapsterMapper;
using MediatR;
using TS.Result;

namespace ERP.Application.Features.RecipeDetails.Create
{
    public sealed record RecipeDetailCreateCommand(
        
        Guid RecipeId,
        Guid ProductId,
        decimal Quantity

        )
        : IRequest<Result<string>>;

    internal sealed class RecipeDetailCreateCommandHandler(
        IRecipeDetailsRepository recipeDetailsRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
        : IRequestHandler<RecipeDetailCreateCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(RecipeDetailCreateCommand request, CancellationToken cancellationToken)
        {

            RecipeDetail? recipeDetail = await recipeDetailsRepository
                .GetByExpressionWithTrackingAsync
                (
                    p => p.RecipeId == request.RecipeId
                    &&
                    p.ProductId == request.ProductId
                    , cancellationToken);

            if(recipeDetail is not null)
            {
                recipeDetail.Quantity += request.Quantity;
            }
            else
            {
                recipeDetail = mapper.Map<RecipeDetail>(request);

                await recipeDetailsRepository.AddAsync(recipeDetail, cancellationToken);

            }

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<string>.Succeed("Recipe successfully saved.");

        }
    }

}
