using DulceFe.API.Services.Domain.Model.Aggregates;
using DulceFe.API.Services.Interfaces.REST.Resources;

namespace DulceFe.API.Services.Interfaces.REST.Transform;

public static class ServiceTransformers
{
    public static CateringInquiry ToEntity(CreateCateringInquiryResource resource)
    {
        return new CateringInquiry
        {
            Name = resource.Name,
            Email = resource.Email,
            Phone = resource.Phone,
            GuestCount = resource.GuestCount,
            EventDate = resource.EventDate,
            EventType = resource.EventType,
            AdditionalDetails = resource.AdditionalDetails
        };
    }

    public static CateringInquiryResource ToResource(CateringInquiry entity)
    {
        return new CateringInquiryResource(entity.Id, entity.Name, entity.Email, entity.Phone, entity.GuestCount, entity.EventDate, entity.EventType, entity.AdditionalDetails, entity.CreatedAt);
    }

    public static ContactMessage ToEntity(CreateContactMessageResource resource)
    {
        return new ContactMessage
        {
            Name = resource.Name,
            Email = resource.Email,
            Subject = resource.Subject,
            Message = resource.Message
        };
    }

    public static ContactMessageResource ToResource(ContactMessage entity)
    {
        return new ContactMessageResource(entity.Id, entity.Name, entity.Email, entity.Subject, entity.Message, entity.CreatedAt);
    }

    public static WorkshopSubscription ToEntity(CreateWorkshopSubscriptionResource resource)
    {
        return new WorkshopSubscription
        {
            Name = resource.Name,
            Email = resource.Email,
            WorkshopId = resource.WorkshopId
        };
    }

    public static WorkshopSubscriptionResource ToResource(WorkshopSubscription entity)
    {
        return new WorkshopSubscriptionResource(entity.Id, entity.Name, entity.Email, entity.WorkshopId, entity.CreatedAt);
    }
}
