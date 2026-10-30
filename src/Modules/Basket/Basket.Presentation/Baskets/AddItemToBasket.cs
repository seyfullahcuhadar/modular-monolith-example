using Basket.Application.Baskets.AddItemToBasket;
using Common.Application.Messaging;
using Common.Presentation.Endpoints;
using Common.Presentation.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Basket.Presentation.Baskets;

internal sealed class AddItemToBasket : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(BasketRoutes.Items, async (
                Guid customerId,
                AddItemToBasketRequest request,
                ICommandHandler<AddItemToBasketCommand> handler,
                CancellationToken cancellationToken) =>
            {
                var result = await handler.Handle(
                    new AddItemToBasketCommand(customerId, request.ProductId, request.Quantity),
                    cancellationToken);

                return result.Match(Results.NoContent, ApiResults.Problem);
            })
            .WithName(nameof(AddItemToBasket))
            .WithTags(BasketRoutes.Tag);
    }

    internal sealed record AddItemToBasketRequest(Guid ProductId, int Quantity);
}
