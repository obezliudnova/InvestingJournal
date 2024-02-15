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
