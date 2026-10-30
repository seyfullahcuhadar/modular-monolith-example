using Common.Domain;

namespace Catalog.Domain.Products;

internal sealed record Money
{
    public const string DefaultCurrency = "TRY";

    private Money(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }

    public decimal Amount { get; }

    public string Currency { get; }

    public static Result<Money> Create(decimal amount, string currency = DefaultCurrency)
    {
        if (currency != DefaultCurrency)
        {
            return CatalogErrors.UnsupportedCurrency(currency);
        }

        if (amount < 0)
        {
            return CatalogErrors.NegativeAmount;
        }

        return new Money(amount, currency);
    }
}
