using DulceFe.API.IAM.Domain.Model.Aggregates;
using DulceFe.API.IAM.Domain.Model.Commands;
using DulceFe.API.IAM.Interfaces.REST.Resources;

namespace DulceFe.API.IAM.Interfaces.REST.Transform;

public static class SignInCommandFromResourceAssembler
{
    public static SignInCommand ToCommandFromResource(SignInResource resource)
    {
        return new SignInCommand(resource.Username, resource.Password);
    }
}

public static class SignUpCommandFromResourceAssembler
{
    public static SignUpCommand ToCommandFromResource(SignUpResource resource)
    {
        return new SignUpCommand(resource.Username, resource.Password);
    }
}

public static class ForgotPasswordCommandFromResourceAssembler
{
    public static ForgotPasswordCommand ToCommandFromResource(ForgotPasswordResource resource)
    {
        return new ForgotPasswordCommand(resource.Email);
    }
}

public static class ResetPasswordCommandFromResourceAssembler
{
    public static ResetPasswordCommand ToCommandFromResource(ResetPasswordResource resource)
    {
        return new ResetPasswordCommand(resource.Token, resource.NewPassword);
    }
}

public static class AuthenticatedUserResourceFromEntityAssembler
{
    public static AuthenticatedUserResource ToResourceFromEntity(User user, string token)
    {
        return new AuthenticatedUserResource(user.Id, user.Username, token);
    }
}

