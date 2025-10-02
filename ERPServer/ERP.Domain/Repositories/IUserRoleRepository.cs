using ERP.Domain.Users;
using GenericRepository;

namespace ERP.Domain.Repositories;
public interface IUserRoleRepository : IRepository<AppUserRole>
{
    Task<List<AppUserRole>> GetAllAppUserRolesAsync(CancellationToken cancellationToken);
}