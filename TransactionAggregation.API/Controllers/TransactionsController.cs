using Microsoft.AspNetCore.Mvc;
using TransactionAggregation.Application.Abstractions;

namespace TransactionAggregation.API.Controllers;

[ApiController]
[Route("api/customers/{customerId}")]
[Produces("application/json")]
public class TransactionsController : ControllerBase
{
    private readonly IAggregationService _aggregationService;
    private readonly ILogger<TransactionsController> _logger;

    public TransactionsController(
      IAggregationService aggregationService,
      ILogger<TransactionsController> logger)
    {
        _aggregationService = aggregationService;
        _logger = logger;
    }

    // Returns all aggregated, categorized transactions for a customer.
    [HttpGet("transactions")]
    public async Task<IActionResult> GetTransactions(string customerId)
    {
        if (string.IsNullOrWhiteSpace(customerId))
            return StatusCode(StatusCodes.Status400BadRequest, "Customer id is required.");

        try
        {
            _logger.LogInformation("Fetching transactions for customer {CustomerId}", customerId);

            var result = await _aggregationService.GetTransactionsAsync(customerId);

            // Return 404 when the customer has no transactions.
            if (result.Count == 0)
                return StatusCode(StatusCodes.Status404NotFound, $"No transactions found for customer {customerId}.");

            return StatusCode(StatusCodes.Status200OK, result);
        }
        catch (Exception ex)
        {
            // Log the full error, but return a safe generic message to the caller.
            _logger.LogError(ex, "Error fetching transactions for customer {CustomerId}", customerId);
            return StatusCode(StatusCodes.Status500InternalServerError,
              "An error occurred while processing your request.");
        }
    }

    // Returns spending totals grouped by category for a customer.
    [HttpGet("categories")]
    public async Task<IActionResult> GetCategorySummary(string customerId)
    {
        if (string.IsNullOrWhiteSpace(customerId))
            return StatusCode(StatusCodes.Status400BadRequest, "Customer id is required.");

        try
        {
            var result = await _aggregationService.GetCategorySummaryAsync(customerId);
            return StatusCode(StatusCodes.Status200OK, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching category summary for customer {CustomerId}", customerId);
            return StatusCode(StatusCodes.Status500InternalServerError,
              "An error occurred while processing your request.");
        }
    }

    // Returns a high-level spending overview (income, spending, net, top category).
    [HttpGet("overview")]
    public async Task<IActionResult> GetOverview(string customerId)
    {
        if (string.IsNullOrWhiteSpace(customerId))
            return StatusCode(StatusCodes.Status400BadRequest, "Customer id is required.");

        try
        {
            var result = await _aggregationService.GetSpendingOverviewAsync(customerId);
            return StatusCode(StatusCodes.Status200OK, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching overview for customer {CustomerId}", customerId);
            return StatusCode(StatusCodes.Status500InternalServerError,
              "An error occurred while processing your request.");
        }
    }
}