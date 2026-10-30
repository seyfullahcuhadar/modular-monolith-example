using Common.Application.Messaging;

namespace Basket.Application.Baskets.AddItemToBasket;

internal sealed record AddItemToBasketCommand(Guid CustomerId, Guid ProductId, int Quantity) : ICommand;
