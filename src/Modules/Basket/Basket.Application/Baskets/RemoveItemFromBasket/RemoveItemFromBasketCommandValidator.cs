using FluentValidation;

namespace Basket.Application.Baskets.RemoveItemFromBasket;

internal sealed class RemoveItemFromBasketCommandValidator : AbstractValidator<RemoveItemFromBasketCommand>
{
    public RemoveItemFromBasketCommandValidator()
    {
        RuleFor(c => c.CustomerId).NotEmpty();
        RuleFor(c => c.ProductId).NotEmpty();
    }
}
