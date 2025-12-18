namespace DulceFe.API.Promotions.Interfaces.REST.Resources;

public record CreateCouponResource(string Code, decimal DiscountValue, bool IsPercentage, DateTime? ExpiryDate, int? AssignedUserId);
public record CouponResource(int Id, string Code, decimal DiscountValue, bool IsPercentage, DateTime? ExpiryDate, int? AssignedUserId, bool IsActive);
public record ValidateCouponResource(string Code);
