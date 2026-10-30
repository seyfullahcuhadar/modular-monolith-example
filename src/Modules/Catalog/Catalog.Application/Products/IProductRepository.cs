using Catalog.Domain.Products;

namespace Catalog.Application.Products;

internal interface IProductRepository
{
    Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<Product> Items, int TotalCount)> GetPageAsync(
        int page,
        int pageSize,
        string? search,
        CancellationToken cancellationToken = default);

    void Add(Product product);
}
