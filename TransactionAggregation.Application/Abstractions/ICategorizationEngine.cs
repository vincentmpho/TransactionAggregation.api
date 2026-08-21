using TransactionAggregation.Domain.Entities;

namespace TransactionAggregation.Application.Abstractions;

// Assigns a category to a transaction based on its details.
public interface ICategorizationEngine
{
    void Categorize(Transaction transaction);
}