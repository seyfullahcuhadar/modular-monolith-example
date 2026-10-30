using FluentValidation;

namespace Catalog.Application.Products.CreateProduct;

internal sealed class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(c => c.Name).NotEmpty().MaximumLength(ProductRules.NameMaxLength);
        RuleFor(c => c.Description).NotNull().MaximumLength(ProductRules.DescriptionMaxLength);
        RuleFor(c => c.Price).MustBeValidPrice();
        RuleFor(c => c.StockQuantity).GreaterThanOrEqualTo(0);
    }
}
