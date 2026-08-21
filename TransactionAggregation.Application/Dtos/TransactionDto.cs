namespace TransactionAggregation.Application.Dtos;

// The shape of a transaction as returned by the API.
public record TransactionDto(
    string Id,
    string CustomerId,
    string Source,
    decimal Amount,
    string Currency,
    string Type,
    DateTimeOffset Timestamp,
    string Description,
    string? MerchantName,
    string Category);