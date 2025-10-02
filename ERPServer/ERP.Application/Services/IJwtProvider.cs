using ERP.Domain.Users;

namespace ERP.Application.Services;

public interface IJwtProvider
{
    public Task<string> CreateTokenAsync(AppUser user, CancellationToken cancellationToken = default);
}