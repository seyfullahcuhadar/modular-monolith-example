using Catalog.Application.Products.AddStock;
using Common.Application.Messaging;
using Common.Presentation.Endpoints;
using Common.Presentation.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Catalog.Presentation.Products;

internal sealed class AddStock : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost($"{ProductRoutes.ProductById}/stock", async (
                Guid id,
                AddStockRequest request,
                ICommandHandler<AddStockCommand> handler,
                CancellationToken cancellationToken) =>
            {
                var result = await handler.Handle(new AddStockCommand(id, request.Quantity), cancellationToken);

                return result.Match(Results.NoContent, ApiResults.Problem);
            })
            .WithName(nameof(AddStock))
            .WithTags(ProductRoutes.Tag);
    }

    internal sealed record AddStockRequest(int Quantity);
}
