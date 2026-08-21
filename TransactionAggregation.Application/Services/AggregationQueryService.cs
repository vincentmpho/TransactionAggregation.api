using TransactionAggregation.Application.Abstractions;
using TransactionAggregation.Application.Dtos;
using TransactionAggregation.Domain.Entities;
using TransactionAggregation.Domain.Enums;

namespace TransactionAggregation.Application.Services;

// Turns aggregated transactions into the shapes the API returns.
public class AggregationQueryService : IAggregationService
{
    private readonly TransactionAggregator _aggregator;

    public AggregationQueryService(TransactionAggregator aggregator)
    {
        _aggregator = aggregator;
    }

    public async Task<IReadOnlyList<TransactionDto>> GetTransactionsAsync(string customerId)
    {
        var transactions = await _aggregator.AggregateAsync(customerId);
        return transactions.Select(ToDto).ToList();
    }

    public async Task<CategorySummaryDto> GetCategorySummaryAsync(string customerId)
    {
        var transactions = await _aggregator.AggregateAsync(customerId);

        // Only money going out counts as "spending"
        var spending = transactions.Where(t => t.Type == TransactionType.Debit);

        var categories = spending
          .GroupBy(t => t.Category)
          .Select(group => new CategoryTotalDto(
            group.Key.ToString(),
            group.Sum(t => t.Amount.Amount),
            group.Count()))
          .OrderByDescending(c => c.TotalSpent)
          .ToList();

        var totalSpent = spending.Sum(t => t.Amount.Amount);

        return new CategorySummaryDto(customerId, totalSpent, categories);
    }

    public async Task<SpendingOverviewDto> GetSpendingOverviewAsync(string customerId)
    {
        var transactions = await _aggregator.AggregateAsync(customerId);

        var totalIncome = transactions
          .Where(t => t.Type == TransactionType.Credit)
          .Sum(t => t.Amount.Amount);

        var totalSpent = transactions
          .Where(t => t.Type == TransactionType.Debit)
          .Sum(t => t.Amount.Amount);

        var topCategory = transactions
          .Where(t => t.Type == TransactionType.Debit)
          .GroupBy(t => t.Category)
          .OrderByDescending(g => g.Sum(t => t.Amount.Amount))
          .Select(g => g.Key.ToString())
          .FirstOrDefault();

        return new SpendingOverviewDto(
          customerId,
          totalIncome,
          totalSpent,
          totalIncome - totalSpent,
          transactions.Count,
          topCategory);
    }

    // Converts a Domain Transaction into an API friendly DTO.
    private static TransactionDto ToDto(Transaction t) => new(
    t.Id,
    t.CustomerId,
    t.Source,
    t.Amount.Amount,
    t.Amount.Currency,
    t.Type.ToString(),
    t.Timestamp,
    t.Description,
    t.MerchantName,
    t.Category.ToString());
}