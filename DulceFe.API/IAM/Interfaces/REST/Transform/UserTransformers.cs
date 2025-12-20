using DulceFe.API.IAM.Domain.Model.Aggregates;
using DulceFe.API.IAM.Domain.Model.Commands;
using DulceFe.API.IAM.Interfaces.REST.Resources;

namespace DulceFe.API.IAM.Interfaces.REST.Transform;

public static class UpdateUserCommandFromResourceAssembler
{
    public static UpdateUserCommand ToCommandFromResource(int id, UpdateUserResource resource)
    {
        return new UpdateUserCommand(id, resource.FirstName, resource.LastName, resource.Email, resource.Phone, resource.Address);
    }
}

public static class UpdateUserPasswordCommandFromResourceAssembler
{
    public static UpdateUserPasswordCommand ToCommandFromResource(int id, UpdateUserPasswordResource resource)
    {
        return new UpdateUserPasswordCommand(id, resource.CurrentPassword, resource.NewPassword);
    }
}

public static class UserResourceFromEntityAssembler
{
    public static UserResource ToResourceFromEntity(User entity)
    {
        return new UserResource(
            entity.Id, 
            entity.Username, 
            entity.FirstName, 
            entity.LastName, 
            entity.Email, 
            entity.Phone, 
            entity.Address,
            entity.Role.ToString()
        );
    }
}
