using DulceFe.API.Sales.Domain.Model.Commands;
using FluentValidation;

namespace DulceFe.API.Sales.Interfaces.REST.Validation;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.UserId).GreaterThan(0).WithMessage("User ID is required.");
        RuleFor(x => x.TotalAmount).GreaterThan(0).WithMessage("Total amount must be greater than zero.");
        RuleFor(x => x.ItemsJson).NotEmpty().WithMessage("Order items cannot be empty.");
    }
}
