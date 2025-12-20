namespace DulceFe.API.Services.Domain.Model.Aggregates;

public class CateringInquiry
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public int GuestCount { get; set; }
    public DateTime EventDate { get; set; }
    public string EventType { get; set; } = string.Empty;
    public string AdditionalDetails { get; set; } = string.Empty;
    public string Status { get; set; } = "Pending";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class ContactMessage
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class WorkshopSubscription
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string WorkshopId { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
