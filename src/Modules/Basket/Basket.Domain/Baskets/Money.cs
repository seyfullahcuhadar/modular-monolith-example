using Common.Domain;

namespace Basket.Domain.Baskets;

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
            return BasketErrors.UnsupportedCurrency(currency);
        }

        if (amount < 0)
        {
            return BasketErrors.NegativeAmount;
        }

        return new Money(amount, currency);
    }

    public Money Multiply(int quantity) => new(Amount * quantity, Currency);
}
