using Common.Application.Messaging;

namespace Basket.Application.Baskets.RemoveItemFromBasket;

internal sealed record RemoveItemFromBasketCommand(Guid CustomerId, Guid ProductId) : ICommand;
