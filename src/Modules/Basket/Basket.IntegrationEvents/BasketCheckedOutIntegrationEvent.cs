using Common.Application.EventBus;

namespace Basket.IntegrationEvents;

public sealed record BasketCheckedOutItem(Guid ProductId, string ProductName, decimal UnitPrice, int Quantity);

public sealed record BasketCheckedOutIntegrationEvent(
    Guid OrderId,
    Guid CustomerId,
    IReadOnlyList<BasketCheckedOutItem> Items,
    decimal TotalAmount) : IntegrationEvent;
