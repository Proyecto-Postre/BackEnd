namespace DulceFe.API.Promotions.Domain.Model.Aggregates;

public class Coupon
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public decimal DiscountValue { get; set; }
    public bool IsPercentage { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public int? AssignedUserId { get; set; } // Null if global
    public bool IsActive { get; set; } = true;
}
