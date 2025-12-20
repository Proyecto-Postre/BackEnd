using DulceFe.API.Promotions.Domain.Model.Commands;
using FluentValidation;

namespace DulceFe.API.Promotions.Interfaces.REST.Validation;

public class CreateCouponCommandValidator : AbstractValidator<CreateCouponCommand>
{
    public CreateCouponCommandValidator()
    {
        RuleFor(x => x.Code).NotEmpty().WithMessage("Code is required.").MinimumLength(3).WithMessage("Code must be at least 3 characters.");
        RuleFor(x => x.DiscountValue).GreaterThan(0).WithMessage("Discount value must be positive.");
    }
}
