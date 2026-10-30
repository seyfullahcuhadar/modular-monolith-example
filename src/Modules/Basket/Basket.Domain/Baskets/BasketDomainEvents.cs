using Common.Domain;

namespace Basket.Domain.Baskets;

internal sealed record CheckedOutItem(Guid ProductId, string ProductName, decimal UnitPrice, int Quantity);

internal sealed record BasketCheckedOutDomainEvent(
    Guid OrderId,
    Guid CustomerId,
    IReadOnlyList<CheckedOutItem> Items,
    decimal TotalAmount) : DomainEvent;
