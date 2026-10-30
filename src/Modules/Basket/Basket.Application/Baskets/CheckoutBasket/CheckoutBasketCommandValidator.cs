using FluentValidation;

namespace Basket.Application.Baskets.CheckoutBasket;

internal sealed class CheckoutBasketCommandValidator : AbstractValidator<CheckoutBasketCommand>
{
    public CheckoutBasketCommandValidator()
    {
        RuleFor(c => c.CustomerId).NotEmpty();
    }
}
