
using ERP.Application.Features.Products.Create;
using ERP.Application.Features.Products.Delete;
using ERP.Application.Features.Products.Update;
using ERP.WebAPI.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ERP.WebAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(IMediator mediator) : ApiController(mediator)
    {

        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductCreateCommand request, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProduct(ProductUpdateCommand request, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteProduct(Guid id, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(new ProductDeleteCommand(id), cancellationToken);
            return StatusCode(response.StatusCode, response);
        }

    }

}