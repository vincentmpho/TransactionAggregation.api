using TransactionAggregation.Domain.Enums;
using TransactionAggregation.Domain.ValueObjects;

namespace TransactionAggregation.Domain.Entities;
public class Transaction
{
    public string Id { get; set; }
    public string CustomerId { get; set; }
    public string Source { get; set; }
    public Money Amount { get; set; }
    public TransactionType Type { get; set; }
    public DateTimeOffset Timestamp { get; set; }
    public string Description { get; set; } 
    public string? MerchantName { get; set; }

    public TransactionCategory Category { get; set; } = TransactionCategory.Uncategorized;
}