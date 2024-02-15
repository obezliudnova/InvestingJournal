using CsvHelper.Configuration;

public record Trade
{
    public string? Order { get; set; }
    public string? AssetCategory { get; set; }
    public string? Currency { get; set; }
    public string? Symbol { get; set; }
    public DateTime DateTime { get; set; }
    public decimal Quantity { get; set; }
    public decimal TPrice { get; set; }
    public decimal Proceeds { get; set; }
    public decimal CommUSD { get; set; }
    public decimal Basis { get; set; }
    public decimal RealizedPL { get; set; }
    public decimal MTMPL { get; set; }
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
