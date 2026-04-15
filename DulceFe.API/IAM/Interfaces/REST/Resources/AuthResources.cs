namespace DulceFe.API.IAM.Interfaces.REST.Resources;

public record SignUpResource(string Username, string Email, string Password, string FirstName = "", string LastName = "", string Phone = "");
public record SignInResource(string UsernameOrEmail, string Password);
public record AuthenticatedUserResource(int Id, string Username, string Token, string Role = "user");
public record ForgotPasswordResource(string Email);
public record ResetPasswordResource(string Token, string NewPassword);
