using Catalog.Application.Products.CreateProduct;
using Common.Application.Messaging;
using Common.Presentation.Endpoints;
using Common.Presentation.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Catalog.Presentation.Products;

internal sealed class CreateProduct : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(ProductRoutes.Products, async (
                CreateProductRequest request,
                ICommandHandler<CreateProductCommand, Guid> handler,
                CancellationToken cancellationToken) =>
            {
                var command = new CreateProductCommand(request.Name, request.Description, request.Price, request.StockQuantity);

                var result = await handler.Handle(command, cancellationToken);

                return result.Match(
                    id => Results.Created($"/{ProductRoutes.Products}/{id}", new { id }),
                    ApiResults.Problem);
            })
            .WithName(nameof(CreateProduct))
            .WithTags(ProductRoutes.Tag);
    }

    internal sealed record CreateProductRequest(string Name, string Description, decimal Price, int StockQuantity);
}
