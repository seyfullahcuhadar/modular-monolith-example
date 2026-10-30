using Common.Application.Messaging;

namespace Catalog.Application.Products.AddStock;

internal sealed record AddStockCommand(Guid ProductId, int Quantity) : ICommand;
