using Catalog.Application.Abstractions;
using Catalog.Domain;
using Catalog.Domain.Products;
using Common.Application.Messaging;
using Common.Domain;

namespace Catalog.Application.Products.UpdateProductPrice;

internal sealed class UpdateProductPriceCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateProductPriceCommand>
{
    public async Task<Result> Handle(UpdateProductPriceCommand command, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetByIdAsync(command.ProductId, cancellationToken);
        if (product is null)
        {
            return CatalogErrors.ProductNotFound(command.ProductId);
        }

        var price = Money.Create(command.Price);
        if (price.IsFailure)
        {
            return price.Error;
        }

        var result = product.ChangePrice(price.Value);
        if (result.IsFailure)
        {
            return result;
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
