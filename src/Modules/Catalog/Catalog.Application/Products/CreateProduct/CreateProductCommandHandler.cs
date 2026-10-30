using Catalog.Application.Abstractions;
using Catalog.Domain.Products;
using Common.Application.Messaging;
using Common.Domain;

namespace Catalog.Application.Products.CreateProduct;

internal sealed class CreateProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<CreateProductCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var price = Money.Create(command.Price);
        if (price.IsFailure)
        {
            return Result.Failure<Guid>(price.Error);
        }

        var product = Product.Create(command.Name, command.Description, price.Value, command.StockQuantity);
        if (product.IsFailure)
        {
            return Result.Failure<Guid>(product.Error);
        }

        productRepository.Add(product.Value);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return product.Value.Id;
    }
}
