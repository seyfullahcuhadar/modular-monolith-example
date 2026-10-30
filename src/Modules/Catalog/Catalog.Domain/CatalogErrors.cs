using Common.Domain;

namespace Catalog.Domain;

internal static class CatalogErrors
{
    public static readonly Error InvalidPrice =
        Error.Validation("Catalog.InvalidPrice", "Price must be greater than zero.");

    public static readonly Error NegativeAmount =
        Error.Validation("Catalog.NegativeAmount", "Amount can't be negative.");

    public static readonly Error NegativeStock =
        Error.Validation("Catalog.NegativeStock", "Stock quantity can't be negative.");

    public static readonly Error InvalidQuantity =
        Error.Validation("Catalog.InvalidQuantity", "Quantity must be greater than zero.");

    public static Error UnsupportedCurrency(string currency) =>
        Error.Validation("Catalog.UnsupportedCurrency", $"Currency '{currency}' is not supported.");

    public static Error ProductNotFound(Guid productId) =>
        Error.NotFound("Catalog.ProductNotFound", $"Product with id '{productId}' was not found.");
}
