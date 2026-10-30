using FluentValidation;

namespace Basket.Application.Baskets.GetBasket;

internal sealed class GetBasketQueryValidator : AbstractValidator<GetBasketQuery>
{
    public GetBasketQueryValidator()
    {
        RuleFor(q => q.CustomerId).NotEmpty();
    }
}
