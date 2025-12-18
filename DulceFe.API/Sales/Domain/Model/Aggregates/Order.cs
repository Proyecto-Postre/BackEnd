namespace DulceFe.API.Sales.Domain.Model.Aggregates;

public class Order
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = "Pending";
    
    // Simplification for now: store items as JSON or reference a separate table
    // In a full DDD approach we would use a collection of OrderItems
    public string ItemsJson { get; set; } = string.Empty; 
    public string CouponCode { get; set; } = string.Empty;
}
