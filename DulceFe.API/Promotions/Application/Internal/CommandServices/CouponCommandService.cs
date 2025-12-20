using DulceFe.API.Promotions.Domain.Model.Aggregates;
using DulceFe.API.Promotions.Domain.Model.Commands;
using DulceFe.API.Promotions.Domain.Repositories;
using DulceFe.API.Promotions.Domain.Services;
using DulceFe.API.Shared.Domain.Repositories;

namespace DulceFe.API.Promotions.Application.Internal.CommandServices;

public class CouponCommandService : ICouponCommandService
{
    private readonly ICouponRepository _couponRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CouponCommandService(ICouponRepository couponRepository, IUnitOfWork unitOfWork)
    {
        _couponRepository = couponRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Coupon> Handle(CreateCouponCommand command)
    {
        var existingCoupon = await _couponRepository.FindByCodeAsync(command.Code);
        if (existingCoupon != null)
            throw new Exception("Coupon code already exists.");

        var coupon = new Coupon
        {
            Code = command.Code,
            DiscountValue = command.DiscountValue,
            IsPercentage = command.IsPercentage,
            ExpiryDate = command.ExpiryDate,
            AssignedUserId = command.AssignedUserId,
            IsActive = true
        };

        await _couponRepository.AddAsync(coupon);
        await _unitOfWork.CompleteAsync();
        return coupon;
    }

    public async Task<Coupon> Handle(UpdateCouponCommand command)
    {
        var coupon = await _couponRepository.FindByIdAsync(command.Id);
        if (coupon == null) throw new Exception("Coupon not found.");

        coupon.Code = command.Code;
        coupon.DiscountValue = command.DiscountValue;
        coupon.IsPercentage = command.IsPercentage;
        coupon.ExpiryDate = command.ExpiryDate;
        coupon.IsActive = command.IsActive;

        _couponRepository.Update(coupon);
        await _unitOfWork.CompleteAsync();
        return coupon;
    }

    public async Task Handle(DeleteCouponCommand command)
    {
        var coupon = await _couponRepository.FindByIdAsync(command.Id);
        if (coupon == null) throw new Exception("Coupon not found.");

        _couponRepository.Remove(coupon);
        await _unitOfWork.CompleteAsync();
    }
}
