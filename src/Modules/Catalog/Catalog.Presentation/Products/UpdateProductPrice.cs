using Catalog.Application.Products.UpdateProductPrice;
using Common.Application.Messaging;
using Common.Presentation.Endpoints;
using Common.Presentation.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Catalog.Presentation.Products;

internal sealed class UpdateProductPrice : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut($"{ProductRoutes.ProductById}/price", async (
                Guid id,
                UpdateProductPriceRequest request,
                ICommandHandler<UpdateProductPriceCommand> handler,
                CancellationToken cancellationToken) =>
            {
                var result = await handler.Handle(new UpdateProductPriceCommand(id, request.Price), cancellationToken);

                return result.Match(Results.NoContent, ApiResults.Problem);
            })
            .WithName(nameof(UpdateProductPrice))
            .WithTags(ProductRoutes.Tag);
    }

    internal sealed record UpdateProductPriceRequest(decimal Price);
}
