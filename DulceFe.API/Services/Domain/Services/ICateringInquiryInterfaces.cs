using DulceFe.API.Services.Domain.Model.Aggregates;

namespace DulceFe.API.Services.Domain.Services;

public interface ICateringInquiryQueryService
{
    Task<IEnumerable<CateringInquiry>> Handle();
}

public interface ICateringInquiryCommandService
{
    Task Handle(int id, string status);
}
