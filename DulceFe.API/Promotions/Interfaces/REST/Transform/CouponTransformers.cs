using DulceFe.API.Promotions.Domain.Model.Aggregates;
using DulceFe.API.Promotions.Domain.Model.Commands;
using DulceFe.API.Promotions.Interfaces.REST.Resources;

namespace DulceFe.API.Promotions.Interfaces.REST.Transform;

public static class CouponResourceFromEntityAssembler
{
    public static CouponResource ToResourceFromEntity(Coupon entity)
    {
        return new CouponResource(entity.Id, entity.Code, entity.DiscountValue, entity.IsPercentage, entity.ExpiryDate, entity.AssignedUserId, entity.IsActive);
    }
}

public static class CreateCouponCommandFromResourceAssembler
{
    public static CreateCouponCommand ToCommandFromResource(CreateCouponResource resource)
    {
        return new CreateCouponCommand(resource.Code, resource.DiscountValue, resource.IsPercentage, resource.ExpiryDate, resource.AssignedUserId);
    }
}

public static class UpdateCouponCommandFromResourceAssembler
{
    public static UpdateCouponCommand ToCommandFromResource(int id, UpdateCouponResource resource)
    {
        return new UpdateCouponCommand(id, resource.Code, resource.DiscountValue, resource.IsPercentage, resource.ExpiryDate, resource.IsActive);
    }
}
