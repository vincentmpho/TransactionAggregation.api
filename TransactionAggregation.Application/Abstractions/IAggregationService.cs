using TransactionAggregation.Application.Dtos;

namespace TransactionAggregation.Application.Abstractions;

// The main read service the API uses to get aggregated data.
public interface IAggregationService
{
    Task<IReadOnlyList<TransactionDto>> GetTransactionsAsync(string customerId);

    Task<CategorySummaryDto> GetCategorySummaryAsync(string customerId);

    Task<SpendingOverviewDto> GetSpendingOverviewAsync(string customerId);
}