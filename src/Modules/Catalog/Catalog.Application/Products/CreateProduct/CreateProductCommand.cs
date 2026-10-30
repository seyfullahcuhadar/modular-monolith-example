using Common.Application.Messaging;

namespace Catalog.Application.Products.CreateProduct;

internal sealed record CreateProductCommand(string Name, string Description, decimal Price, int StockQuantity)
    : ICommand<Guid>;
