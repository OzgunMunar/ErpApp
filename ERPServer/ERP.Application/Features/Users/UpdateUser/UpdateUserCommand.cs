using TS.MediatR;
using TS.Result;

namespace ERP.Application.Features.Users.UpdateUser;

public sealed record UserUpdateCommand(

    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string UserName,
    string Password,
    Guid RoleId

) : IRequest<Result<string>>;
