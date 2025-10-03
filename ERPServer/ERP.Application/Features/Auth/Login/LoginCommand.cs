using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.Result;

namespace ERP.Application.Features.Auth.Login
{
    public sealed record LoginCommand(
        string emailOrUserName,
        string password
    ) : IRequest<Result<LoginCommandResponse>>;

}
