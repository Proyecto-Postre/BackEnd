using DulceFe.API.Promotions.Domain.Model.Aggregates;
using DulceFe.API.Promotions.Domain.Model.Queries;
using DulceFe.API.Promotions.Domain.Repositories;
using DulceFe.API.Promotions.Domain.Services;

namespace DulceFe.API.Promotions.Application.Internal.QueryServices;

public class CouponQueryService : ICouponQueryService
{
    private readonly ICouponRepository _couponRepository;

    public CouponQueryService(ICouponRepository couponRepository)
    {
        _couponRepository = couponRepository;
    }

    public async Task<IEnumerable<Coupon>> Handle(GetAllCouponsQuery query)
    {
        return await _couponRepository.ListAsync();
    }

    public async Task<Coupon?> Handle(GetCouponByIdQuery query)
    {
        return await _couponRepository.FindByIdAsync(query.Id);
    }

    public async Task<Coupon?> Handle(GetCouponByCodeQuery query)
    {
        return await _couponRepository.FindByCodeAsync(query.Code);
    }
}
