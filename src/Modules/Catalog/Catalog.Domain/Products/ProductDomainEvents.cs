using Common.Domain;

namespace Catalog.Domain.Products;

internal sealed record ProductCreatedDomainEvent(Guid ProductId) : DomainEvent;

internal sealed record ProductPriceChangedDomainEvent(Guid ProductId, decimal OldPrice, decimal NewPrice) : DomainEvent;
