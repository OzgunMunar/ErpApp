using ERP.Application.Features.Customers.Create;
using ERP.Application.Features.Customers.Delete;
using ERP.Application.Features.Customers.Update;
using ERP.Application.Features.Depot.Create;
using ERP.Application.Features.Depot.Delete;
using ERP.Application.Features.Depot.Update;
using ERP.WebAPI.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ERP.WebAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class DepotController(IMediator mediator) : ApiController(mediator)
    {

        [HttpPost]
        public async Task<IActionResult> CreateDepot(DepotCreateCommand request, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateDepot(DepotUpdateCommand request, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteDepot(Guid id, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(new DepotDeleteCommand(id), cancellationToken);
            return StatusCode(response.StatusCode, response);
        }

    }

}