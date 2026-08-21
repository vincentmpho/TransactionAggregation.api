namespace TransactionAggregation.Application.Dtos;

// Totals for a single category.
public record CategoryTotalDto(
    string Category,
    decimal TotalSpent,
    int TransactionCount);

// A summary of spending grouped by category.
public record CategorySummaryDto(
    string CustomerId,
    decimal TotalSpent,
    IReadOnlyList<CategoryTotalDto> Categories);