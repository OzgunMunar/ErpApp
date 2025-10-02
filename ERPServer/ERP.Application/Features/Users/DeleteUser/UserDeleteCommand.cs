using ERP.Domain.Users;
using GenericRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TS.MediatR;
using TS.Result;

namespace ERPServer.Application.Features.Users.DeleteUser;

public sealed record UserDeleteCommand(
    Guid Id
): IRequest<Result<string>>;

internal sealed class DeleteUserCommandHandler(

    UserManager<AppUser> userManager,
    IUnitOfWork unitOfWork

)
: IRequestHandler<UserDeleteCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UserDeleteCommand request, CancellationToken cancellationToken)
    {

        AppUser? appUser = await userManager.Users
                            .Where(user => user.Id == request.Id)
                            .FirstOrDefaultAsync(cancellationToken);

        if (appUser is null)
        {
            return Result<string>.Failure("User could not be found");
        }

        appUser.IsActive = false;

        await userManager.UpdateAsync(appUser);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("User is deleted.");

    }
}