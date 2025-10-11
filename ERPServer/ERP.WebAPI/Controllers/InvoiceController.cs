using ERP.Application.Features.Invoices.Create;
using ERP.Application.Features.Invoices.Delete;
using ERP.Application.Features.Invoices.Update;
using ERP.WebAPI.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ERP.WebAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController(IMediator mediator) : ApiController(mediator)
    {

        [HttpPost]
        public async Task<IActionResult> CreateInvoice(InvoiceCreateCommand request, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateInvoice(InvoiceUpdateCommand request, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteInvoice(Guid id, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(new InvoiceDeleteCommand(id), cancellationToken);
            return StatusCode(response.StatusCode, response);
        }

    }

}