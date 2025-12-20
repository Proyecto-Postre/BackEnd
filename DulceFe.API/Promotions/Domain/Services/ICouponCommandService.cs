using DulceFe.API.Promotions.Domain.Model.Aggregates;
using DulceFe.API.Promotions.Domain.Model.Commands;

namespace DulceFe.API.Promotions.Domain.Services;

public interface ICouponCommandService
{
    Task<Coupon> Handle(CreateCouponCommand command);
    Task<Coupon> Handle(UpdateCouponCommand command);
    Task Handle(DeleteCouponCommand command);
}
