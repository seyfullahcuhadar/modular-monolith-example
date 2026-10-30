using Catalog.Application.Products;
using Catalog.Application.Products.GetProducts;
using Common.Application.Messaging;
using Common.Application.Paging;
using Common.Presentation.Endpoints;
using Common.Presentation.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Catalog.Presentation.Products;

internal sealed class GetProducts : IEndpoint
{
    private const int DefaultPageSize = 20;

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(ProductRoutes.Products, async (
                IQueryHandler<GetProductsQuery, PagedResult<ProductResponse>> handler,
                CancellationToken cancellationToken,
                int page = 1,
                int pageSize = DefaultPageSize,
                string? search = null) =>
            {
                var result = await handler.Handle(new GetProductsQuery(page, pageSize, search), cancellationToken);

                return result.Match(products => Results.Ok(products), ApiResults.Problem);
            })
            .WithName(nameof(GetProducts))
            .WithTags(ProductRoutes.Tag);
    }
}
