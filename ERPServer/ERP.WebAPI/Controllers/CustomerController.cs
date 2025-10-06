using ERP.Application.Features.Customers.Create;
using ERP.Application.Features.Customers.Delete;
using ERP.Application.Features.Customers.Update;
using ERP.WebAPI.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ERP.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController(IMediator mediator) : ApiController(mediator)
    {
        [HttpPost]
        public async Task<IActionResult> CreateCustomer(CustomerCreateCommand request, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCustomer(CustomerUpdateCommand request, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteCustomer(Guid id, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(new CustomerDeleteCommand(id), cancellationToken);
            return StatusCode(response.StatusCode, response);
        }

    }

}