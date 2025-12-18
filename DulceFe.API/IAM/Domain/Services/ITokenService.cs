using DulceFe.API.IAM.Domain.Model.Aggregates;

namespace DulceFe.API.IAM.Domain.Services;

public interface ITokenService
{
    string GenerateToken(User user);
    int? ValidateToken(string token);
}
