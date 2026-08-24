using TransactionAggregation.Application.DataSources;
using TransactionAggregation.Domain.Entities;
using TransactionAggregation.Domain.Enums;
using TransactionAggregation.Domain.ValueObjects;

namespace TransactionAggregation.Infrastructure.DataSources;

// Matches the fields in wallet.json.
internal class WalletRecord
{
    public string Id { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public bool Topup { get; set; }
    public DateTimeOffset When { get; set; }
    public string Note { get; set; } = string.Empty;
}

// Reads wallet.json and turns each record into a Transaction.
public class WalletDataSource : ITransactionDataSource
{
    public string SourceName => "Wallet";

    public async Task<IReadOnlyCollection<Transaction>> GetTransactionsAsync(string customerId)
    {
        var records = await MockFileReader.ReadAsync<WalletRecord>("wallet.json");

        var transactions = new List<Transaction>();

        foreach (var record in records)
        {
            // Skip records that belong to a different customer.
            if (record.UserId != customerId)
                continue;

            // this topup  means money came in; false means money went out.
            var type = record.Topup ? TransactionType.Credit : TransactionType.Debit;

            var transaction = new Transaction
            {
                Id = record.Id,
                CustomerId = customerId,
                Source = SourceName,
                Amount = new Money(record.Value, "ZAR"),
                Type = type,
                Timestamp = record.When,
                Description = record.Note
            };

            transactions.Add(transaction);
        }

        return transactions;
    }
}