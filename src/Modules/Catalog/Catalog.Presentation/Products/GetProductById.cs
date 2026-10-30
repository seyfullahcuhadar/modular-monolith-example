using Catalog.Application.Products;
using Catalog.Application.Products.GetProductById;
using Common.Application.Messaging;
using Common.Presentation.Endpoints;
using Common.Presentation.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Catalog.Presentation.Products;

internal sealed class GetProductById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(ProductRoutes.ProductById, async (
                Guid id,
                IQueryHandler<GetProductByIdQuery, ProductResponse> handler,
                CancellationToken cancellationToken) =>
            {
                var result = await handler.Handle(new GetProductByIdQuery(id), cancellationToken);

                return result.Match(product => Results.Ok(product), ApiResults.Problem);
            })
            .WithName(nameof(GetProductById))
            .WithTags(ProductRoutes.Tag);
    }
}
