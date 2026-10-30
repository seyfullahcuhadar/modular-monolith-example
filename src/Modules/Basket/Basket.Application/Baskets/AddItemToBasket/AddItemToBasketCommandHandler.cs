using Basket.Application.Abstractions;
using Basket.Domain;
using Basket.Domain.Baskets;
using Catalog.PublicApi;
using Common.Application.Messaging;
using Common.Domain;

namespace Basket.Application.Baskets.AddItemToBasket;

internal sealed class AddItemToBasketCommandHandler(
    IBasketRepository basketRepository,
    ICatalogApi catalogApi,
    IUnitOfWork unitOfWork)
    : ICommandHandler<AddItemToBasketCommand>
{
    public async Task<Result> Handle(AddItemToBasketCommand command, CancellationToken cancellationToken)
    {
        var product = await catalogApi.GetProductAsync(command.ProductId, cancellationToken);
        if (product is null)
        {
            return BasketErrors.ProductNotFound(command.ProductId);
        }

        var unitPrice = Money.Create(product.Price);
        if (unitPrice.IsFailure)
        {
            return unitPrice.Error;
        }

        var basket = await basketRepository.GetByCustomerIdAsync(command.CustomerId, cancellationToken);
        if (basket is null)
        {
            basket = Domain.Baskets.Basket.Create(command.CustomerId);
            basketRepository.Add(basket);
        }

        var result = basket.AddItem(
            product.Id,
            product.Name,
            unitPrice.Value,
            command.Quantity,
            product.AvailableQuantity);

        if (result.IsFailure)
        {
            return result;
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
