using ERP.Application.Features.Auth.Login;
using ERP.WebAPI.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TS.Result;

namespace ERP.WebAPI.Controllers
{
    public class AuthController(IMediator mediator) : ApiController(mediator)
    {
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginCommand request, CancellationToken cancellationToken)
        {
            Result<LoginCommandResponse> response = await _mediator.Send(request, cancellationToken);

            return StatusCode(response.StatusCode, response);
        }
    }
}
