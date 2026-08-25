using TransactionAggregation.Application.Services;
using TransactionAggregation.Domain.Entities;
using TransactionAggregation.Domain.Enums;
using TransactionAggregation.Domain.ValueObjects;
using Xunit;

namespace TransactionAggregation.UnitTests;

public class CategorizationEngineTests
{
    [Fact]
    public void Categorize_GroceriesKeyword_SetsGroceriesCategory()
    {
        // Arrange
        var engine = new CategorizationEngine();
        var transaction = new Transaction
        {
            Id = "1",
            CustomerId = "CUST-001",
            Source = "Test",
            Amount = new Money(100, "ZAR"),
            Type = TransactionType.Debit,
            Timestamp = DateTimeOffset.Now,
            Description = "CHECKERS HYPER GROCERIES"
        };

        // Act
        engine.Categorize(transaction);

        // Assert
        Assert.Equal(TransactionCategory.Groceries, transaction.Category);
    }

    [Fact]
    public void Categorize_UnknownDescription_SetsUncategorized()
    {
        // Arrange
        var engine = new CategorizationEngine();
        var transaction = new Transaction
        {
            Id = "2",
            CustomerId = "CUST-001",
            Source = "Test",
            Amount = new Money(100, "ZAR"),
            Type = TransactionType.Debit,
            Timestamp = DateTimeOffset.Now,
            Description = "some random unknown text"
        };

        // Act
        engine.Categorize(transaction);

        // Assert
        Assert.Equal(TransactionCategory.Uncategorized, transaction.Category);
    }
}