namespace Catalog.Application.Abstractions;

internal interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
