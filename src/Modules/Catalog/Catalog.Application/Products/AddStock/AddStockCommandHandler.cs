using Catalog.Application.Abstractions;
using Catalog.Domain;
using Common.Application.Messaging;
using Common.Domain;

namespace Catalog.Application.Products.AddStock;

internal sealed class AddStockCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<AddStockCommand>
{
    public async Task<Result> Handle(AddStockCommand command, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetByIdAsync(command.ProductId, cancellationToken);
        if (product is null)
        {
            return CatalogErrors.ProductNotFound(command.ProductId);
        }

        var result = product.AddStock(command.Quantity);
        if (result.IsFailure)
        {
            return result;
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
