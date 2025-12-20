namespace DulceFe.API.IAM.Domain.Model.Commands;

public record SignUpCommand(string Username, string Password);
public record SignInCommand(string Username, string Password);
public record ForgotPasswordCommand(string Email);
public record ResetPasswordCommand(string Token, string NewPassword);
