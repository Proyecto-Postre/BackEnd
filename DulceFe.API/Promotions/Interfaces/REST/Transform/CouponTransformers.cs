using DulceFe.API.Promotions.Domain.Model.Aggregates;
using DulceFe.API.Promotions.Interfaces.REST.Resources;

namespace DulceFe.API.Promotions.Interfaces.REST.Transform;

public static class CouponTransformers
{
    public static Coupon ToEntity(CreateCouponResource resource)
    {
        return new Coupon
        {
            Code = resource.Code,
            DiscountValue = resource.DiscountValue,
            IsPercentage = resource.IsPercentage,
            ExpiryDate = resource.ExpiryDate,
            AssignedUserId = resource.AssignedUserId
        };
    }

    public static CouponResource ToResource(Coupon entity)
    {
        return new CouponResource(entity.Id, entity.Code, entity.DiscountValue, entity.IsPercentage, entity.ExpiryDate, entity.AssignedUserId, entity.IsActive);
    }
}
