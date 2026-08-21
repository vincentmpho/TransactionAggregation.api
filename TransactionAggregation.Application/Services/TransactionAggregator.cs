using TransactionAggregation.Application.Abstractions;
using TransactionAggregation.Application.DataSources;
using TransactionAggregation.Domain.Entities;

namespace TransactionAggregation.Application.Services;

// Pulls transactions from every data source, combines them  and categorizes each one.
public class TransactionAggregator
{
    private readonly IEnumerable<ITransactionDataSource> _sources;
    private readonly ICategorizationEngine _categorizationEngine;

    public TransactionAggregator(
        IEnumerable<ITransactionDataSource> sources,
        ICategorizationEngine categorizationEngine)
    {
        _sources = sources;
        _categorizationEngine = categorizationEngine;
    }

    public async Task<IReadOnlyList<Transaction>> AggregateAsync(string customerId)
    {
        var allTransactions = new List<Transaction>();

        // Ask every source for this customer's transactions.
        foreach (var source in _sources)
        {
            try
            {
                var transactions = await source.GetTransactionsAsync(customerId);
                allTransactions.AddRange(transactions);
            }
            catch
            {
                // If one source fails, skip it instead of breaking everything.
            }
        }

        // Categorize every transaction.
        foreach (var transaction in allTransactions)
        {
            _categorizationEngine.Categorize(transaction);
        }

        // Newest transactions first.
        return allTransactions
            .OrderByDescending(t => t.Timestamp)
            .ToList();
    }
}