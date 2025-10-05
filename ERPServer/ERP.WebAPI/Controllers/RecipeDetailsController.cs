
using ERP.Application.Features.RecipeDetails.Create;
using ERP.Application.Features.RecipeDetails.Delete;
using ERP.Application.Features.RecipeDetails.GetRecipeByIdWithDetails;
using ERP.Application.Features.RecipeDetails.Update;
using ERP.WebAPI.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ERP.WebAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class RecipeDetailsController(IMediator mediator) : ApiController(mediator)
    {

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetRecipeByIdWithDetails(Guid id, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(new GetRecipeByIdWithDetailsQuery(id), cancellationToken);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRecipeDetail(RecipeDetailCreateCommand request, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteRecipeDetail(Guid id, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(new RecipeDetailDeleteCommand(id), cancellationToken);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateRecipeDetail(RecipeDetailUpdateCommand request, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return StatusCode(response.StatusCode, response);
        }

    }

}