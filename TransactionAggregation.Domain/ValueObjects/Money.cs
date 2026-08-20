namespace TransactionAggregation.Domain.ValueObjects;

public class Money
{
    public decimal Amount { get; }
    public string Currency { get; }

    public Money(decimal amount, string currency)
    {
        if (string.IsNullOrWhiteSpace(currency))
            throw new ArgumentException("Currency is required.");

        Amount = amount;
        Currency = currency;
    }

    public override string ToString()
    {
        return $"{Amount:0.00} {Currency}";
    }
}