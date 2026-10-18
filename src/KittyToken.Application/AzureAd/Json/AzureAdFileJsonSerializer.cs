using System.Text.Json;

namespace KittyToken.Application.AzureAd.Json;

public static class AzureAdFileJsonSerializer
{
    public static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true
    };

    public static string Serialize<TValue>(
        TValue value)
    {
        return JsonSerializer.Serialize(value, Options);
    }

    public static TValue? Deserialize<TValue>(
        string json)
    {
        return JsonSerializer.Deserialize<TValue>(json, Options);
    }
}
