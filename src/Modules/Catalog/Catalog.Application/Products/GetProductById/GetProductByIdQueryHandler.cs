using Catalog.Domain;
using Common.Application.Messaging;
using Common.Domain;

namespace Catalog.Application.Products.GetProductById;

internal sealed class GetProductByIdQueryHandler(IProductRepository productRepository)
    : IQueryHandler<GetProductByIdQuery, ProductResponse>
{
    public async Task<Result<ProductResponse>> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetByIdAsync(query.ProductId, cancellationToken);

        return product is null
            ? CatalogErrors.ProductNotFound(query.ProductId)
            : ProductResponse.FromProduct(product);
    }
}
