using DulceFe.API.Promotions.Domain.Model.Aggregates;
using DulceFe.API.Promotions.Domain.Repositories;
using DulceFe.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using DulceFe.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DulceFe.API.Promotions.Infrastructure.Persistence.EFC.Repositories;

public class CouponRepository : BaseRepository<Coupon>, ICouponRepository
{
    public CouponRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Coupon?> FindByCodeAsync(string code)
    {
        return await Context.Set<Coupon>()
            .FirstOrDefaultAsync(c => c.Code == code);
    }
}
