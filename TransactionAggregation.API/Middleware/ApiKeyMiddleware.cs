namespace TransactionAggregation.API.Middleware;

// Rejects any request that does not include a valid API key and  in the "X-Api-Key" header, returning 401 Unauthorized.
public class ApiKeyMiddleware
{
    private const string HeaderName = "X-Api-Key";
    private readonly RequestDelegate _next;
    private readonly string _expectedApiKey;

    public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        // The key is read from configuration
        _expectedApiKey = configuration["ApiKey"] ?? string.Empty;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path.Value ?? string.Empty;

        if (path.Contains("swagger") || path == "/" || path.StartsWith("/health"))
        {
            await _next(context);
            return;
        }

        var hasKey = context.Request.Headers.TryGetValue(HeaderName, out var providedKey);

        if (!hasKey || providedKey != _expectedApiKey)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Missing or invalid API key.");
            return;
        }

        await _next(context);
    }
}