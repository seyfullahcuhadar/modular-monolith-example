namespace Catalog.PublicApi;

public interface ICatalogApi
{
    Task<ProductDto?> GetProductAsync(Guid id, CancellationToken cancellationToken = default);
}

public sealed record ProductDto(Guid Id, string Name, decimal Price, int AvailableQuantity);
