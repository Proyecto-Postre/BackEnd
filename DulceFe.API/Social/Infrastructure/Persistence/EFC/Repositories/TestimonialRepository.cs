using DulceFe.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using DulceFe.API.Social.Domain.Model.Aggregates;
using DulceFe.API.Social.Domain.Repositories;
using DulceFe.API.Shared.Infrastructure.Persistence.EFC.Configuration;

namespace DulceFe.API.Social.Infrastructure.Persistence.EFC.Repositories;

public class TestimonialRepository : BaseRepository<Testimonial>, ITestimonialRepository
{
    public TestimonialRepository(AppDbContext context) : base(context)
    {
    }
}
