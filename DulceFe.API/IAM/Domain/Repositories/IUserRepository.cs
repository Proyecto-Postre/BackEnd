using DulceFe.API.IAM.Domain.Model.Aggregates;
using DulceFe.API.Shared.Domain.Repositories;

namespace DulceFe.API.IAM.Domain.Repositories;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User?> FindByUsernameAsync(string username);
    bool ExistsByUsername(string username);
    Task<User?> FindByEmailAsync(string email);
    Task<User?> FindByPasswordResetTokenAsync(string token);
}
