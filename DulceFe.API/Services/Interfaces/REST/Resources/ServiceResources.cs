using DulceFe.API.Services.Domain.Model.Aggregates;

namespace DulceFe.API.Services.Interfaces.REST.Resources;

public record CreateCateringInquiryResource(string Name, string Email, string Phone, int GuestCount, DateTime EventDate, string EventType, string AdditionalDetails);
public record CreateContactMessageResource(string Name, string Email, string Subject, string Message);

public record CateringInquiryResource(int Id, string Name, string Email, string Phone, int GuestCount, DateTime EventDate, string EventType, string AdditionalDetails, DateTime CreatedAt);
public record ContactMessageResource(int Id, string Name, string Email, string Subject, string Message, DateTime CreatedAt);
public record CreateWorkshopSubscriptionResource(string Name, string Email, string WorkshopId);
public record WorkshopSubscriptionResource(int Id, string Name, string Email, string WorkshopId, DateTime CreatedAt);
