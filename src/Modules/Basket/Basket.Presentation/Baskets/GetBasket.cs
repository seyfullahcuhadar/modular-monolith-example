using Basket.Application.Baskets;
using Basket.Application.Baskets.GetBasket;
using Common.Application.Messaging;
using Common.Presentation.Endpoints;
using Common.Presentation.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Basket.Presentation.Baskets;

internal sealed class GetBasket : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(BasketRoutes.Basket, async (
                Guid customerId,
                IQueryHandler<GetBasketQuery, BasketResponse> handler,
                CancellationToken cancellationToken) =>
            {
                var result = await handler.Handle(new GetBasketQuery(customerId), cancellationToken);

                return result.Match(basket => Results.Ok(basket), ApiResults.Problem);
            })
            .WithName(nameof(GetBasket))
            .WithTags(BasketRoutes.Tag);
    }
}
