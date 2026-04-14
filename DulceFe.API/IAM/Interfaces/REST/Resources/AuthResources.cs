namespace DulceFe.API.IAM.Interfaces.REST.Resources;

public record SignUpResource(string Username, string Email, string Password);
public record SignInResource(string Email, string Password);
public record AuthenticatedUserResource(int Id, string Username, string Token);
public record ForgotPasswordResource(string Email);
public record ResetPasswordResource(string Token, string NewPassword);
