using Moq;
using TransactionAggregation.Application.Abstractions;
using TransactionAggregation.Application.DataSources;
using TransactionAggregation.Application.Services;
using TransactionAggregation.Domain.Entities;
using TransactionAggregation.Domain.Enums;
using TransactionAggregation.Domain.ValueObjects;
using Xunit;

namespace TransactionAggregation.UnitTests;

public class TransactionAggregatorTests
{
    [Fact]
    public async Task AggregateAsync_CombinesTransactionsFromAllSources()
    {
        // Arrange create two fake data sources using Moq
        var source1 = new Mock<ITransactionDataSource>();
        source1.Setup(s => s.SourceName).Returns("Source1");
        source1.Setup(s => s.GetTransactionsAsync(It.IsAny<string>()))
            .ReturnsAsync(new List<Transaction> { MakeTransaction("1") });

        var source2 = new Mock<ITransactionDataSource>();
        source2.Setup(s => s.SourceName).Returns("Source2");
        source2.Setup(s => s.GetTransactionsAsync(It.IsAny<string>()))
            .ReturnsAsync(new List<Transaction> { MakeTransaction("2") });

        var categorizationEngine = new CategorizationEngine();

        var aggregator = new TransactionAggregator(
            new[] { source1.Object, source2.Object },
            categorizationEngine);

        // Act
        var result = await aggregator.AggregateAsync("CUST-001");

        // Assert  both sources transactions should be combined
        Assert.Equal(2, result.Count);
    }

    // Helper to build a test transaction.
    private static Transaction MakeTransaction(string id) => new()
    {
        Id = id,
        CustomerId = "CUST-001",
        Source = "Test",
        Amount = new Money(100, "ZAR"),
        Type = TransactionType.Debit,
        Timestamp = DateTimeOffset.Now,
        Description = "test transaction"
    };
}