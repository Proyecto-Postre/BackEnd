using DulceFe.API.Promotions.Domain.Model.Aggregates;
using DulceFe.API.Promotions.Domain.Model.Queries;

namespace DulceFe.API.Promotions.Domain.Services;

public interface ICouponQueryService
{
    Task<IEnumerable<Coupon>> Handle(GetAllCouponsQuery query);
    Task<Coupon?> Handle(GetCouponByIdQuery query);
    Task<Coupon?> Handle(GetCouponByCodeQuery query);
}
