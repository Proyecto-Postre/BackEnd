namespace DulceFe.API.Promotions.Domain.Model.Commands;

public record CreateCouponCommand(string Code, decimal DiscountValue, bool IsPercentage, DateTime? ExpiryDate, int? AssignedUserId);
public record UpdateCouponCommand(int Id, string Code, decimal DiscountValue, bool IsPercentage, DateTime? ExpiryDate, bool IsActive);
public record DeleteCouponCommand(int Id);
