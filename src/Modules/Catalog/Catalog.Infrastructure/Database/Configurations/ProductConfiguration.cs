using Catalog.Application.Products;
using Catalog.Domain.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Infrastructure.Database.Configurations;

internal sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("products", t =>
        {
            t.HasCheckConstraint("ck_products_stock_quantity_non_negative", "stock_quantity >= 0");
            t.HasCheckConstraint("ck_products_reserved_quantity_within_stock", "reserved_quantity >= 0 AND reserved_quantity <= stock_quantity");
        });

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).ValueGeneratedNever();

        builder.Property(p => p.Name).HasMaxLength(ProductRules.NameMaxLength);
        builder.Property(p => p.Description).HasMaxLength(ProductRules.DescriptionMaxLength);

        builder.ComplexProperty(p => p.Price, price =>
        {
            price.Property(m => m.Amount).HasPrecision(18, 2);
            price.Property(m => m.Currency).HasMaxLength(3);
        });

        builder.Ignore(p => p.AvailableQuantity);
        builder.Ignore(p => p.DomainEvents);

        builder.HasIndex(p => p.Name);
    }
}
