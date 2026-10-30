using Common.Application.Messaging;

namespace Catalog.Application.Products.UpdateProductPrice;

internal sealed record UpdateProductPriceCommand(Guid ProductId, decimal Price) : ICommand;
