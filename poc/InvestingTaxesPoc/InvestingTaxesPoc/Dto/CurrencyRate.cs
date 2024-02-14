using System.Text.Json.Serialization;

public record CurrencyRate
{
    [JsonPropertyName("txt")]
    public string? Name { get; init; }
    [JsonPropertyName("rate")]
    public double Rate { get; init; }
    [JsonPropertyName("cc")]
    public string? Code { get; init; }
    [JsonPropertyName("exchangedate")]
    public DateOnly ExchangeDate { get; init; }

    public override sealed string ToString() => $"{Code},{ExchangeDate},{Rate}";
};

