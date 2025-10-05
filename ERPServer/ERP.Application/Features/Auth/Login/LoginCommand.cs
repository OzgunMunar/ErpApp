using MediatR;
using TS.Result;

namespace ERP.Application.Features.Auth.Login
{
    public sealed record LoginCommand(
        string emailOrUserName,
        string password
    ) : IRequest<Result<LoginCommandResponse>>;

}
