namespace TransactionAggregation.Application.Dtos;

// A high-level overview of a customer's money in and out.
public record SpendingOverviewDto(
    string CustomerId,
    decimal TotalIncome,
    decimal TotalSpent,
    decimal Net,
    int TransactionCount,
    string? TopCategory);