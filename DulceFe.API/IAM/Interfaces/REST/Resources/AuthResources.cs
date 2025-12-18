namespace DulceFe.API.IAM.Interfaces.REST.Resources;

public record UserResource(int Id, string Username);
public record SignUpResource(string Username, string Password);
public record SignInResource(string Username, string Password);
public record AuthenticatedUserResource(int Id, string Username, string Token);
