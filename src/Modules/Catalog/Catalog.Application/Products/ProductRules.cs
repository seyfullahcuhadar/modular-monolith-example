using FluentValidation;

namespace Catalog.Application.Products;

internal static class ProductRules
{
    public const int NameMaxLength = 200;

    public const int DescriptionMaxLength = 2000;

    public static IRuleBuilderOptions<T, decimal> MustBeValidPrice<T>(this IRuleBuilder<T, decimal> rule) =>
        rule.GreaterThan(0).PrecisionScale(18, 2, ignoreTrailingZeros: true);
}
