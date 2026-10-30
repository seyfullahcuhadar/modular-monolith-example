using Basket.Application.Abstractions;
using Basket.Domain;
using Common.Application.Messaging;
using Common.Domain;

namespace Basket.Application.Baskets.CheckoutBasket;

internal sealed class CheckoutBasketCommandHandler(IBasketRepository basketRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<CheckoutBasketCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CheckoutBasketCommand command, CancellationToken cancellationToken)
    {
        var basket = await basketRepository.GetByCustomerIdAsync(command.CustomerId, cancellationToken);
        if (basket is null)
        {
            return BasketErrors.EmptyBasket;
        }

        var result = basket.Checkout();
        if (result.IsFailure)
        {
            return result;
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return result.Value;
    }
}
