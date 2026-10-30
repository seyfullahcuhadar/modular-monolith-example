using Common.Domain;

namespace Basket.Domain;

internal static class BasketErrors
{
    public static readonly Error InvalidQuantity =
        Error.Validation("Basket.InvalidQuantity", "Quantity must be greater than zero.");

    public static readonly Error NegativeAmount =
        Error.Validation("Basket.NegativeAmount", "Amount can't be negative.");

    public static readonly Error EmptyBasket =
        Error.Conflict("Basket.EmptyBasket", "An empty basket can't be checked out.");

    public static Error UnsupportedCurrency(string currency) =>
        Error.Validation("Basket.UnsupportedCurrency", $"Currency '{currency}' is not supported.");

    public static Error ProductNotFound(Guid productId) =>
        Error.NotFound("Basket.ProductNotFound", $"Product with id '{productId}' was not found.");

    public static Error ItemNotFound(Guid productId) =>
        Error.NotFound("Basket.ItemNotFound", $"Basket item with product id '{productId}' was not found.");

    public static Error InsufficientStock(int availableQuantity) =>
        Error.Conflict("Basket.InsufficientStock", $"Only {availableQuantity} item(s) are available.");
}
