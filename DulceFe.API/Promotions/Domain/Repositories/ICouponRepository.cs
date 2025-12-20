using DulceFe.API.Promotions.Domain.Model.Aggregates;
using DulceFe.API.Shared.Domain.Repositories;

namespace DulceFe.API.Promotions.Domain.Repositories;

public interface ICouponRepository : IBaseRepository<Coupon>
{
    Task<Coupon?> FindByCodeAsync(string code);
}
