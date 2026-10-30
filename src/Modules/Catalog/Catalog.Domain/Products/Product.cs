using Common.Domain;

namespace Catalog.Domain.Products;

internal sealed class Product : AggregateRoot<Guid>
{
    private Product(Guid id, string name, string description, Money price, int stockQuantity)
        : base(id)
    {
        Name = name;
        Description = description;
        Price = price;
        StockQuantity = stockQuantity;
    }

    // EF Core materialization
    private Product()
    {
        Name = null!;
        Description = null!;
        Price = null!;
    }

    public string Name { get; private set; }

    public string Description { get; private set; }

    public Money Price { get; private set; }

    public int StockQuantity { get; private set; }

    public int ReservedQuantity { get; private set; }

    public int AvailableQuantity => StockQuantity - ReservedQuantity;

    public static Result<Product> Create(string name, string description, Money price, int stockQuantity)
    {
        if (price.Amount <= 0)
        {
            return CatalogErrors.InvalidPrice;
        }

        if (stockQuantity < 0)
        {
            return CatalogErrors.NegativeStock;
        }

        var product = new Product(Guid.CreateVersion7(), name.Trim(), description.Trim(), price, stockQuantity);

        product.Raise(new ProductCreatedDomainEvent(product.Id));

        return product;
    }

    public Result ChangePrice(Money newPrice)
    {
        if (newPrice.Amount <= 0)
        {
            return CatalogErrors.InvalidPrice;
        }

        if (newPrice == Price)
        {
            return Result.Success();
        }

        var oldPrice = Price;
        Price = newPrice;

        Raise(new ProductPriceChangedDomainEvent(Id, oldPrice.Amount, newPrice.Amount));

        return Result.Success();
    }

    public Result AddStock(int quantity)
    {
        if (quantity <= 0)
        {
            return CatalogErrors.InvalidQuantity;
        }

        StockQuantity += quantity;

        return Result.Success();
    }
}
