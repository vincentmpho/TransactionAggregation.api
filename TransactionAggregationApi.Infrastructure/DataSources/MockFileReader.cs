using System.Text.Json;

namespace TransactionAggregation.Infrastructure.DataSources;

// Reads a JSON file from the MockData  and converts it  into a list of the given type.
internal static class MockFileReader
{
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static async Task<List<T>> ReadAsync<T>(string fileName)
    {
        var path = Path.Combine(AppContext.BaseDirectory, "MockData", fileName);

        if (!File.Exists(path))
            return new List<T>();

        await using var stream = File.OpenRead(path);
        var items = await JsonSerializer.DeserializeAsync<List<T>>(stream, Options);
        return items ?? new List<T>();
    }
}