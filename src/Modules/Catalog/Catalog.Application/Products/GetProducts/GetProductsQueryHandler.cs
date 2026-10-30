using Common.Application.Messaging;
using Common.Application.Paging;
using Common.Domain;

namespace Catalog.Application.Products.GetProducts;

internal sealed class GetProductsQueryHandler(IProductRepository productRepository)
    : IQueryHandler<GetProductsQuery, PagedResult<ProductResponse>>
{
    public async Task<Result<PagedResult<ProductResponse>>> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await productRepository.GetPageAsync(
            query.Page,
            query.PageSize,
            query.Search,
            cancellationToken);

        return new PagedResult<ProductResponse>(
            items.Select(ProductResponse.FromProduct).ToList(),
            query.Page,
            query.PageSize,
            totalCount);
    }
}
