using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Basket.Infrastructure.Database.Configurations;

internal sealed class BasketConfiguration : IEntityTypeConfiguration<Domain.Baskets.Basket>
{
    public void Configure(EntityTypeBuilder<Domain.Baskets.Basket> builder)
    {
        builder.ToTable("baskets");

        builder.HasKey(b => b.Id);
        builder.Property(b => b.Id).ValueGeneratedNever();

        builder.Ignore(b => b.CustomerId);
        builder.Ignore(b => b.TotalAmount);
        builder.Ignore(b => b.DomainEvents);

        builder.OwnsMany(b => b.Items, items =>
        {
            items.ToTable("basket_items");
            items.WithOwner().HasForeignKey("BasketId");
            items.Property<int>("Id").ValueGeneratedOnAdd();
            items.HasKey("Id");

            items.Property(i => i.ProductId);
            items.Property(i => i.ProductName).HasMaxLength(200);
            items.Property(i => i.Quantity);

            items.OwnsOne(i => i.UnitPrice, price =>
            {
                price.Property(m => m.Amount).HasPrecision(18, 2);
                price.Property(m => m.Currency).HasMaxLength(3);
            });

            items.Ignore(i => i.LineTotal);

            items.HasIndex("BasketId", nameof(Domain.Baskets.BasketItem.ProductId)).IsUnique();
        });

        builder.Navigation(b => b.Items).HasField("_items").UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
