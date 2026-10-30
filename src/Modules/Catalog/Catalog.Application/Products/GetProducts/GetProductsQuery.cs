using Common.Application.Messaging;
using Common.Application.Paging;

namespace Catalog.Application.Products.GetProducts;

internal sealed record GetProductsQuery(int Page, int PageSize, string? Search) : IQuery<PagedResult<ProductResponse>>;
