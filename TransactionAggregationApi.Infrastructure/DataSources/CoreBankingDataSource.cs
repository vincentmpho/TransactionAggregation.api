using TransactionAggregation.Application.DataSources;
using TransactionAggregation.Domain.Entities;
using TransactionAggregation.Domain.Enums;
using TransactionAggregation.Domain.ValueObjects;

namespace TransactionAggregation.Infrastructure.DataSources;

// Matches the fields in core-banking.json.
internal class CoreBankingRecord
{
    public string Reference { get; set; } = string.Empty;
    public string AccountHolder { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public DateTimeOffset BookedAt { get; set; }
    public string Narrative { get; set; } = string.Empty;
}

// Reads core-banking.json and turns each record into a Transaction.
public class CoreBankingDataSource : ITransactionDataSource
{
    public string SourceName => "CoreBanking";

    public async Task<IReadOnlyCollection<Transaction>> GetTransactionsAsync(string customerId)
    {
        var records = await MockFileReader.ReadAsync<CoreBankingRecord>("core-banking.json");

        var transactions = new List<Transaction>();

        foreach (var record in records)
        {
            // Skip records that belong to a different customer.
            if (record.AccountHolder != customerId)
                continue;

            // A negative amount means money went out (Debit).
            var isMoneyOut = record.Amount < 0;

            var transaction = new Transaction
            {
                Id = record.Reference,
                CustomerId = customerId,
                Source = SourceName,
                Amount = new Money(Math.Abs(record.Amount), record.Currency),
                Type = isMoneyOut ? TransactionType.Debit : TransactionType.Credit,
                Timestamp = record.BookedAt,
                Description = record.Narrative
            };

            transactions.Add(transaction);
        }

        return transactions;
    }
}