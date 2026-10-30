using Common.Application.Messaging;

namespace Catalog.Application.Products.GetProductById;

internal sealed record GetProductByIdQuery(Guid ProductId) : IQuery<ProductResponse>;
