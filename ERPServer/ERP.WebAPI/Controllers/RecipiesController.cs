
using ERP.Application.Features.Recipies.Create;
using ERP.Application.Features.Recipies.Delete;
using ERP.WebAPI.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ERP.WebAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class RecipiesController(IMediator mediator) : ApiController(mediator)
    {

        [HttpPost]
        public async Task<IActionResult> CreatRecipe(RecipeCreateCommand request, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteRecipe(Guid id, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(new RecipeDeleteCommand(id), cancellationToken);
            return StatusCode(response.StatusCode, response);
        }

    }

}