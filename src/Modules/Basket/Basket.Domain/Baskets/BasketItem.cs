namespace Basket.Domain.Baskets;

internal sealed class BasketItem
{
    internal BasketItem(Guid productId, string productName, Money unitPrice, int quantity)
    {
        ProductId = productId;
        ProductName = productName;
        UnitPrice = unitPrice;
        Quantity = quantity;
    }

    // EF Core materialization
    private BasketItem()
    {
        ProductName = null!;
        UnitPrice = null!;
    }

    public Guid ProductId { get; private set; }

    public string ProductName { get; private set; }

    public Money UnitPrice { get; private set; }

    public int Quantity { get; private set; }

    public Money LineTotal => UnitPrice.Multiply(Quantity);

    internal void IncreaseQuantity(int quantity) => Quantity += quantity;
}
