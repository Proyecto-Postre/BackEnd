namespace DulceFe.API.IAM.Interfaces.REST.Resources;

public record UserResource(
    int Id, 
    string Username, 
    string FirstName, 
    string LastName, 
    string Email, 
    string Phone, 
    string Address,
    string Role
);

public record UpdateUserResource(
    string FirstName, 
    string LastName, 
    string Email, 
    string Phone, 
    string Address
);

public record UpdateUserPasswordResource(
    string CurrentPassword, 
    string NewPassword
);
