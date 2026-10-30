using Basket.Application.Baskets.RemoveItemFromBasket;
using Common.Application.Messaging;
using Common.Presentation.Endpoints;
using Common.Presentation.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Basket.Presentation.Baskets;

internal sealed class RemoveItemFromBasket : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete(BasketRoutes.ItemByProduct, async (
                Guid customerId,
                Guid productId,
                ICommandHandler<RemoveItemFromBasketCommand> handler,
                CancellationToken cancellationToken) =>
            {
                var result = await handler.Handle(
                    new RemoveItemFromBasketCommand(customerId, productId),
                    cancellationToken);

                return result.Match(Results.NoContent, ApiResults.Problem);
            })
            .WithName(nameof(RemoveItemFromBasket))
            .WithTags(BasketRoutes.Tag);
    }
}
