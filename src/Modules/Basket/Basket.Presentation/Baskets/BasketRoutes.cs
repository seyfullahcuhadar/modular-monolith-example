namespace Basket.Presentation.Baskets;

internal static class BasketRoutes
{
    public const string Tag = "Basket";

    public const string Basket = "basket/{customerId:guid}";

    public const string Items = "basket/{customerId:guid}/items";

    public const string ItemByProduct = "basket/{customerId:guid}/items/{productId:guid}";

    public const string Checkout = "basket/{customerId:guid}/checkout";
}
