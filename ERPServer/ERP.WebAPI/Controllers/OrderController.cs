using ERP.Application.Features.Orders.Create;
using ERP.Application.Features.Orders.Delete;
using ERP.Application.Features.Orders.RequirementsPlanningByOrderId;
using ERP.Application.Features.Orders.Update;
using ERP.WebAPI.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ERP.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController(IMediator mediator) : ApiController(mediator)
    {
        [HttpPost]
        public async Task<IActionResult> CreateOrder(OrderCreateCommand request, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateOrder(OrderUpdateCommand request, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteOrder(Guid id, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(new OrderDeleteCommand(id), cancellationToken);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("requirements-planning")]
        public async Task<IActionResult> RequirementsPlanningByOrderId(
            [FromBody]RequirementsPlanningByOrderIdCommand request, 
            CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return StatusCode(response.StatusCode, response);
        }

    }
}
