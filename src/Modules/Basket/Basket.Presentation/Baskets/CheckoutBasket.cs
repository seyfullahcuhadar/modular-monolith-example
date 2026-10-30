using Basket.Application.Baskets.CheckoutBasket;
using Common.Application.Messaging;
using Common.Presentation.Endpoints;
using Common.Presentation.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Basket.Presentation.Baskets;

internal sealed class CheckoutBasket : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(BasketRoutes.Checkout, async (
                Guid customerId,
                ICommandHandler<CheckoutBasketCommand, Guid> handler,
                CancellationToken cancellationToken) =>
            {
                var result = await handler.Handle(new CheckoutBasketCommand(customerId), cancellationToken);

                return result.Match(
                    orderId => Results.Accepted($"/orders/{orderId}", new { orderId }),
                    ApiResults.Problem);
            })
            .WithName(nameof(CheckoutBasket))
            .WithTags(BasketRoutes.Tag);
    }
}
