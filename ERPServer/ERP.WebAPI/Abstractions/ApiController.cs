using Microsoft.AspNetCore.Mvc;
using TS.MediatR;

namespace ERP.WebAPI.Abstractions;

[Route("api/[controller]/[action]")]
[ApiController]
public abstract class ApiController : ControllerBase
{
    public readonly ISender _sender;
    public ApiController(ISender sender)
    {
        _sender = sender;
    }
}