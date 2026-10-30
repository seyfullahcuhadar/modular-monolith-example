using Common.Domain;

namespace Basket.Domain.Baskets;

internal sealed class Basket : AggregateRoot<Guid>
{
    private readonly List<BasketItem> _items = [];

    private Basket(Guid customerId)
        : base(customerId)
    {
    }

    // EF Core materialization
    private Basket()
    {
    }

    public Guid CustomerId => Id;

    public IReadOnlyCollection<BasketItem> Items => _items.AsReadOnly();

    public decimal TotalAmount => _items.Sum(i => i.LineTotal.Amount);

    public static Basket Create(Guid customerId) => new(customerId);

    public Result AddItem(Guid productId, string productName, Money unitPrice, int quantity, int availableQuantity)
    {
        if (quantity <= 0)
        {
            return BasketErrors.InvalidQuantity;
        }

        var existing = _items.FirstOrDefault(i => i.ProductId == productId);
        var newQuantity = (existing?.Quantity ?? 0) + quantity;

        if (newQuantity > availableQuantity)
        {
            return BasketErrors.InsufficientStock(availableQuantity);
        }

        if (existing is not null)
        {
            existing.IncreaseQuantity(quantity);
            return Result.Success();
        }

        _items.Add(new BasketItem(productId, productName, unitPrice, quantity));

        return Result.Success();
    }

    public Result RemoveItem(Guid productId)
    {
        var item = _items.FirstOrDefault(i => i.ProductId == productId);
        if (item is null)
        {
            return BasketErrors.ItemNotFound(productId);
        }

        _items.Remove(item);

        return Result.Success();
    }

    public Result<Guid> Checkout()
    {
        if (_items.Count == 0)
        {
            return BasketErrors.EmptyBasket;
        }

        var orderId = Guid.CreateVersion7();
        var snapshot = _items
            .Select(i => new CheckedOutItem(i.ProductId, i.ProductName, i.UnitPrice.Amount, i.Quantity))
            .ToList();

        Raise(new BasketCheckedOutDomainEvent(orderId, CustomerId, snapshot, TotalAmount));

        _items.Clear();

        return orderId;
    }
}
