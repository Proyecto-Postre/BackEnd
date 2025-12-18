using DulceFe.API.IAM.Domain.Model.Aggregates;
using DulceFe.API.IAM.Domain.Model.Commands;

namespace DulceFe.API.IAM.Domain.Services;

public interface IUserCommandService
{
    Task<User?> Handle(SignUpCommand command);
    Task<(User user, string token)> Handle(SignInCommand command);
}
