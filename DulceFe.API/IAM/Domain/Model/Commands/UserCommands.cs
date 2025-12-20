namespace DulceFe.API.IAM.Domain.Model.Commands;

public record UpdateUserCommand(int Id, string FirstName, string LastName, string Email, string Phone, string Address);
public record UpdateUserPasswordCommand(int Id, string CurrentPassword, string NewPassword);
