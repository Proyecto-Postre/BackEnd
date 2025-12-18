namespace DulceFe.API.IAM.Domain.Model.Queries;

public record GetUserByIdQuery(int Id);
public record GetUserByUsernameQuery(string Username);
public record GetAllUsersQuery();
