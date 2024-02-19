using System.Text;

public class FinantialStatement
{
    public List<Trade> Trades { get; set; } = [];

    public List<Dividend> Dividends { get; set; } = [];

    public override string ToString()
    {
        var sb = new StringBuilder();
        Trades.ForEach(trade => { sb.AppendLine(trade.ToString()); });
        Dividends.ForEach(dividend => sb.AppendLine(dividend.ToString()));
        return sb.ToString();
    }
}

public class FinantialStatementTaxes
{
    public List<DividendTax> DividendTax { get; set; } = [];

    public double TotalPersonalIncomeTax => DividendTax.Sum(d => d.PersonalIncomeTax);
    public double MilitaryDutyTax => DividendTax.Sum(d => d.MilitaryDutyTax);

    public override string ToString()
    {
        var sb = new StringBuilder();
        DividendTax.ForEach(dividend => sb.AppendLine(dividend.ToString()));
        sb.AppendLine($"Total Personal Income Tax: {TotalPersonalIncomeTax}");
        sb.AppendLine($"Total Military Duty Tax: {MilitaryDutyTax}");
        return sb.ToString();
    }
}
