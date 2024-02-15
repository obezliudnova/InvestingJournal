using CsvHelper.Configuration;

public record Dividend
{
    public string? Currency { get; set; }
    public DateTime Date { get; set; }
    public string? Description { get; set; }
    public decimal Amount { get; set; }

    public override sealed string ToString() => $"Dividend: {Description} | {Date} | {Amount} | {Currency}";

}

public sealed class DividendMap : ClassMap<Dividend>
{
    public DividendMap()
    {
        Map(m => m.Currency);
        Map(m => m.Date);
        Map(m => m.Amount);
        Map(m => m.Description);
    }
}

