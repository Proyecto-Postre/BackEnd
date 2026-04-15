namespace DulceFe.API.IAM.Domain.Model.Commands;

public record SignUpCommand(string Username, string Email, string Password, string FirstName = "", string LastName = "", string Phone = "");
public record SignInCommand(string UsernameOrEmail, string Password);
public record ForgotPasswordCommand(string Email);
public record ResetPasswordCommand(string Token, string NewPassword);
