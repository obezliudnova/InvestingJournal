<Query Kind="Statements">
  <Namespace>System.Net.Http</Namespace>
  <Namespace>System.Text.Json.Serialization</Namespace>
</Query>


using System.Text.Json;

using HttpClient httpClient = new()
{
    BaseAddress = new Uri("https://bank.gov.ua"),
};
var options = new JsonSerializerOptions()
{
	PropertyNameCaseInsensitive = true
};
options.Converters.Add(new DateOnlyConverter());

using HttpResponseMessage response = await httpClient.GetAsync("NBUStatService/v1/statdirectory/exchangenew?json&date=20240211");
response.EnsureSuccessStatusCode();

var jsonResponse = await response.Content.ReadAsStringAsync();
//Console.WriteLine($"{jsonResponse}\n");

List<CurrencyRate> rates = JsonSerializer.Deserialize<List<CurrencyRate>>(jsonResponse, options);
rates.Where(r => r.Code == "USD").Dump();



public record CurrencyRate 
{
	[JsonPropertyName("txt")]
	public string Name { get; init; }
	[JsonPropertyName("rate")]
	public double Rate { get; init; }
	[JsonPropertyName("cc")]
	public string Code { get; init; }
	[JsonPropertyName("exchangedate")]
	public DateOnly ExchangeDate { get; init; }
};

public class DateOnlyConverter : JsonConverter<DateOnly>
{
    private readonly string serializationFormat;

    public DateOnlyConverter() : this(null)
    {
    }

    public DateOnlyConverter(string? serializationFormat)
    {
        this.serializationFormat = serializationFormat ?? "yyyy-MM-dd";
    }

    public override DateOnly Read(ref Utf8JsonReader reader, 
                            Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return DateOnly.ParseExact(value!, "dd.MM.yyyy");
    }

    public override void Write(Utf8JsonWriter writer, DateOnly value, 
                                        JsonSerializerOptions options)
        => writer.WriteStringValue(value.ToString(serializationFormat));
}