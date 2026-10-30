using Basket.Application.Abstractions;
using Basket.Domain;
using Common.Application.Messaging;
using Common.Domain;

namespace Basket.Application.Baskets.RemoveItemFromBasket;

internal sealed class RemoveItemFromBasketCommandHandler(IBasketRepository basketRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<RemoveItemFromBasketCommand>
{
    public async Task<Result> Handle(RemoveItemFromBasketCommand command, CancellationToken cancellationToken)
    {
        var basket = await basketRepository.GetByCustomerIdAsync(command.CustomerId, cancellationToken);
        if (basket is null)
        {
            return BasketErrors.ItemNotFound(command.ProductId);
        }

        var result = basket.RemoveItem(command.ProductId);
        if (result.IsFailure)
        {
            return result;
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
