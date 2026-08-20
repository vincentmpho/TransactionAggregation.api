using TransactionAggregation.Domain.Entities;

namespace TransactionAggregation.Application.DataSources;

public interface ITransactionDataSource
{
    string SourceName { get; }

    // Fetches all transactions for a given customer from this source.
    Task<IReadOnlyCollection<Transaction>> GetTransactionsAsync(string customerId);
}