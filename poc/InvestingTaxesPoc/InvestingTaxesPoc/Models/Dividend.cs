using CsvHelper.Configuration;

public record Dividend
{
    public string Currency { get; set; } = "";
    public DateTime Date { get; set; }
    public string Description { get; set; } = "";
    public double Amount { get; set; }

    public override sealed string ToString() => $"Dividend: {Description} | {Date} | {Amount} | {Currency}";

}

public class DividendTax
{
    public Dividend Dividend { get; private set; }
    public double CurrencyRate { get; private set; }
    public double Amount => CurrencyRate * Dividend.Amount;
    public double PersonalIncomeTax => Amount * 0.09;
    public double MilitaryDutyTax => Amount * 0.015;
    public override sealed string ToString() => $"{Dividend} | Rate: {CurrencyRate} | {Amount} | PersonalIncomeTax: {PersonalIncomeTax} | MilitaryDutyTax: {MilitaryDutyTax}";

    public DividendTax(Dividend dividend, double rate)
    {
        Dividend = dividend;
        CurrencyRate = rate;
    }
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

