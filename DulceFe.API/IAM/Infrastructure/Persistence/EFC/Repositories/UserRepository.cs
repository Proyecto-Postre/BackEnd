using DulceFe.API.IAM.Domain.Model.Aggregates;
using DulceFe.API.IAM.Domain.Repositories;
using DulceFe.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using DulceFe.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DulceFe.API.IAM.Infrastructure.Persistence.EFC.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<User?> FindByUsernameAsync(string username)
    {
        return await Context.Set<User>().FirstOrDefaultAsync(u => u.Username == username);
    }

    public bool ExistsByUsername(string username)
    {
        return Context.Set<User>().Any(u => u.Username == username);
    }

    public async Task<User?> FindByEmailAsync(string email)
    {
        return await Context.Set<User>().FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> FindByPasswordResetTokenAsync(string token)
    {
        return await Context.Set<User>().FirstOrDefaultAsync(u => u.PasswordResetToken == token);
    }
}
