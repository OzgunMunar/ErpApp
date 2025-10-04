using ERP.Domain.Entities;
using ERP.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.Result;

namespace ERP.Application.Features.Recipies.GetAll
{
    public sealed record GetAllRecipeQuery()
        : IRequest<Result<List<Recipe>>>;

    internal sealed class GetAllRecipeQueryHandler(
        IRecipeRepository recipeRepository)
        : IRequestHandler<GetAllRecipeQuery, Result<List<Recipe>>>
    {
        public async Task<Result<List<Recipe>>> Handle(GetAllRecipeQuery request, CancellationToken cancellationToken)
        {

            List<Recipe> recipes = await recipeRepository
                .GetAll()
                .Include(p => p.Product)
                .OrderBy(p => p.Product!.ProductName)
                .ToListAsync(cancellationToken);

            return recipes;

        }
    }
}
