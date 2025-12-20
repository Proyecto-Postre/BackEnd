namespace DulceFe.API.Sales.Domain.Model.Queries;

public record GetAllOrdersQuery();
public record GetOrderByIdQuery(int Id);
public record GetOrdersByUserIdQuery(int UserId);
public record GetOrdersByUserIdPagedQuery(int UserId, int Page, int PageSize);
