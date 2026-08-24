using TransactionAggregation.Application.DataSources;
using TransactionAggregation.Domain.Entities;
using TransactionAggregation.Domain.Enums;
using TransactionAggregation.Domain.ValueObjects;

namespace TransactionAggregation.Infrastructure.DataSources;

// Matches the fields in card-processor.json.
internal class CardProcessorRecord
{
    public string TxnId { get; set; } = string.Empty;
    public string Customer { get; set; } = string.Empty;
    public long AmountCents { get; set; }
    public string Ccy { get; set; } = string.Empty;
    public string Direction { get; set; } = string.Empty;
    public DateTimeOffset Timestamp { get; set; }
    public string Merchant { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

// Reads card-processor.json and turns each record into a Transaction.
public class CardProcessorDataSource : ITransactionDataSource
{
    public string SourceName => "CardProcessor";

    public async Task<IReadOnlyCollection<Transaction>> GetTransactionsAsync(string customerId)
    {
        var records = await MockFileReader.ReadAsync<CardProcessorRecord>("card-processor.json");

        var transactions = new List<Transaction>();

        foreach (var record in records)
        {
            // Skip records that belong to a different customer.
            if (record.Customer != customerId)
                continue;

            // The amount is in cents, so divide by 100 to get the real value.
            var amount = record.AmountCents / 100m;

            // The direction is given as text which  means money in.
            var isMoneyIn = record.Direction == "CREDIT";

            var transaction = new Transaction
            {
                Id = record.TxnId,
                CustomerId = customerId,
                Source = SourceName,
                Amount = new Money(amount, record.Ccy),
                Type = isMoneyIn ? TransactionType.Credit : TransactionType.Debit,
                Timestamp = record.Timestamp,
                Description = record.Description,
                MerchantName = record.Merchant
            };

            transactions.Add(transaction);
        }

        return transactions;
    }
}