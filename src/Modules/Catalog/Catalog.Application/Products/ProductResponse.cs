using Catalog.Domain.Products;

namespace Catalog.Application.Products;

internal sealed record ProductResponse(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    string Currency,
    int StockQuantity,
    int ReservedQuantity,
    int AvailableQuantity)
{
    public static ProductResponse FromProduct(Product product) => new(
        product.Id,
        product.Name,
        product.Description,
        product.Price.Amount,
        product.Price.Currency,
        product.StockQuantity,
        product.ReservedQuantity,
        product.AvailableQuantity);
}
