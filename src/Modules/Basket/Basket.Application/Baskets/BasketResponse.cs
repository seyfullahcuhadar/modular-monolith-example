namespace Basket.Application.Baskets;

internal sealed record BasketItemResponse(
    Guid ProductId,
    string ProductName,
    decimal UnitPrice,
    string Currency,
    int Quantity,
    decimal LineTotal);

internal sealed record BasketResponse(Guid CustomerId, IReadOnlyList<BasketItemResponse> Items, decimal TotalAmount)
{
    public static BasketResponse Empty(Guid customerId) => new(customerId, [], 0);

    public static BasketResponse FromBasket(Domain.Baskets.Basket basket) => new(
        basket.CustomerId,
        basket.Items
            .Select(i => new BasketItemResponse(
                i.ProductId,
                i.ProductName,
                i.UnitPrice.Amount,
                i.UnitPrice.Currency,
                i.Quantity,
                i.LineTotal.Amount))
            .ToList(),
        basket.TotalAmount);
}
