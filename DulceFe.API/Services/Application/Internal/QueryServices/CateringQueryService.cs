using DulceFe.API.Services.Domain.Model.Aggregates;
using DulceFe.API.Services.Domain.Repositories;
using DulceFe.API.Services.Domain.Services;

namespace DulceFe.API.Services.Application.Internal.QueryServices;

public class CateringQueryService : ICateringInquiryQueryService
{
    private readonly ICateringInquiryRepository _repository;

    public CateringQueryService(ICateringInquiryRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<CateringInquiry>> Handle()
    {
        return await _repository.ListAsync();
    }
}
