using ERP.Domain.Entities;
using ERP.Domain.Repositories;
using GenericRepository;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace ERP.Application.Features.RecipeDetails.Update
{
    public sealed record RecipeDetailUpdateCommand(
        Guid Id,
        Guid ProductId,
        decimal Quantity)
        : IRequest<Result<string>>;

    internal sealed class RecipeDetailUpdateCommandHandler(
        IRecipeDetailsRepository recipeDetailsRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
        : IRequestHandler<RecipeDetailUpdateCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(RecipeDetailUpdateCommand request, CancellationToken cancellationToken)
        {
            
            RecipeDetail? recipeDetail = await recipeDetailsRepository
                .GetByExpressionWithTrackingAsync(p => p.Id == request.Id, cancellationToken);

            if(recipeDetail == null)
            {
                return Result<string>.Failure(404, "Recipe not found.");
            }

            RecipeDetail? oldRecipeDetail = await recipeDetailsRepository
                .Where(
                    p => p.Id != request.Id
                    &&
                    p.ProductId == request.ProductId
                    &&
                    p.RecipeId == recipeDetail.RecipeId)
                .FirstOrDefaultAsync(cancellationToken);

            if(oldRecipeDetail != null)
            {
                recipeDetail.IsDeleted = true;

                oldRecipeDetail.Quantity += request.Quantity;
                recipeDetailsRepository.Update(oldRecipeDetail);
            }
            else
            {
                mapper.Map(request, oldRecipeDetail);
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<string>.Succeed("Recipe successfully updated.");

        }

    }

}