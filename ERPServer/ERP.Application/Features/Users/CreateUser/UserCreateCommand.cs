using TS.MediatR;
using TS.Result;

namespace ERPServer.Application.Features.Users.CreateUser;

public sealed record UserCreateCommand(

    string FirstName,
    string LastName,
    string Email,
    string UserName,
    string Password,
    Guid RoleId

)
:IRequest<Result<string>>;
