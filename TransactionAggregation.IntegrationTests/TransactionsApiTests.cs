using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace TransactionAggregation.IntegrationTests;

public class TransactionsApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public TransactionsApiTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetTransactions_WithoutApiKey_Returns401()
    {
        // Act - call the endpoint with no API key
        var response = await _client.GetAsync("/api/customers/CUST-001/transactions");

        // Assert - should be rejected
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetTransactions_WithApiKey_Returns200()
    {
        // Arrange - add the API key header
        _client.DefaultRequestHeaders.Add("X-Api-Key", "my-local-dev-key-12345");

        // Act
        var response = await _client.GetAsync("/api/customers/CUST-001/transactions");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetHealth_ReturnsHealthy()
    {
        // Act - health endpoint needs no key
        var response = await _client.GetAsync("/health");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}