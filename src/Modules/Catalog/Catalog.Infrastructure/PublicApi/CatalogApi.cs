using Catalog.Application.Products;
using Catalog.Application.Products.GetProductById;
using Catalog.PublicApi;
using Common.Application.Messaging;

namespace Catalog.Infrastructure.PublicApi;

internal sealed class CatalogApi(IQueryHandler<GetProductByIdQuery, ProductResponse> getProductById) : ICatalogApi
{
    public async Task<ProductDto?> GetProductAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await getProductById.Handle(new GetProductByIdQuery(id), cancellationToken);

        if (result.IsFailure)
        {
            return null;
        }

        var product = result.Value;

        return new ProductDto(product.Id, product.Name, product.Price, product.AvailableQuantity);
    }
}
