using DulceFe.API.Services.Domain.Model.Aggregates;
using DulceFe.API.Services.Domain.Repositories;
using DulceFe.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using DulceFe.API.Shared.Infrastructure.Persistence.EFC.Repositories;

namespace DulceFe.API.Services.Infrastructure.Persistence.EFC.Repositories;

public class CateringInquiryRepository : BaseRepository<CateringInquiry>, ICateringInquiryRepository
{
    public CateringInquiryRepository(AppDbContext context) : base(context)
    {
    }
}
