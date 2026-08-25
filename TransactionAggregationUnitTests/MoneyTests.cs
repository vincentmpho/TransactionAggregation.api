using TransactionAggregation.Domain.ValueObjects;
using Xunit;

namespace TransactionAggregation.UnitTests;

public class MoneyTests
{
    [Fact]
    public void Constructor_ValidValues_StoresAmountAndCurrency()
    {
        // Arrange and Act
        var money = new Money(150.50m, "ZAR");

        // Assert
        Assert.Equal(150.50m, money.Amount);
        Assert.Equal("ZAR", money.Currency);
    }

    [Fact]
    public void Constructor_EmptyCurrency_ThrowsException()
    {
        // Act and Assert
        Assert.Throws<ArgumentException>(() => new Money(100, ""));
    }
}