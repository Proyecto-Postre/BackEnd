using DulceFe.API.Sales.Domain.Model.Aggregates;

namespace DulceFe.API.Sales.Domain.Model.Commands;

public record CreateOrderCommand(int UserId, decimal TotalAmount, string ItemsJson, string CouponCode);
public record CancelOrderCommand(int OrderId);
