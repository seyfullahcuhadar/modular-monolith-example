using FluentValidation;

namespace Catalog.Application.Products.GetProducts;

internal sealed class GetProductsQueryValidator : AbstractValidator<GetProductsQuery>
{
    public const int MaxPageSize = 100;

    public GetProductsQueryValidator()
    {
        RuleFor(q => q.Page).GreaterThanOrEqualTo(1);
        RuleFor(q => q.PageSize).InclusiveBetween(1, MaxPageSize);
        RuleFor(q => q.Search).MaximumLength(ProductRules.NameMaxLength);
    }
}
