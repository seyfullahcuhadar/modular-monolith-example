using FluentValidation;

namespace Catalog.Application.Products.UpdateProductPrice;

internal sealed class UpdateProductPriceCommandValidator : AbstractValidator<UpdateProductPriceCommand>
{
    public UpdateProductPriceCommandValidator()
    {
        RuleFor(c => c.ProductId).NotEmpty();
        RuleFor(c => c.Price).MustBeValidPrice();
    }
}
