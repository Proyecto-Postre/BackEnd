using DulceFe.API.IAM.Application.Internal.OutboundServices;
using BC = BCrypt.Net.BCrypt;

namespace DulceFe.API.IAM.Infrastructure.Hashing.BCrypt.Services;

public class HashingService : IHashingService
{
    public string HashPassword(string password)
    {
        return BC.HashPassword(password);
    }

    public bool VerifyPassword(string password, string hashedPassword)
    {
        return BC.Verify(password, hashedPassword);
    }
}
