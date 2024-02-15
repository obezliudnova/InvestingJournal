using CsvHelper.Configuration;

public record Trade
{
    public string? Order { get; set; }
    public string? AssetCategory { get; set; }
    public string? Currency { get; set; }
    public string? Symbol { get; set; }
    public DateTime DateTime { get; set; }
    public double Quantity { get; set; }
    public double TPrice { get; set; }
    public double Proceeds { get; set; }
    public double CommUSD { get; set; }
    public double Basis { get; set; }
    public double RealizedPL { get; set; }
    public double MTMPL { get; set; }
    public string? Code { get; set; }

    public override sealed string ToString() => $"Trade: {AssetCategory} | {Symbol} | {DateTime} | {TPrice} | {Currency} | {CommUSD}";
}

public sealed class TradeMap : ClassMap<Trade>
{
    public TradeMap()
    {
        Map(m => m.Currency).Name("Currency");
        Map(m => m.DateTime).Name("Date/Time");
        Map(m => m.TPrice).Name("T. Price");
        Map(m => m.Symbol);
        Map(m => m.CommUSD).Name("Comm/Fee");
        Map(m => m.AssetCategory).Name("Asset Category");
    }
}
