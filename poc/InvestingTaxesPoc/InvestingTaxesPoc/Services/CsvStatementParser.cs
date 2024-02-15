using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

public class CsvStatementParser : IStatementParser
{
    private readonly CsvConfiguration _config;

    public CsvStatementParser()
    {
        _config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            IgnoreBlankLines = false,
        };
    }

    public FinantialStatement Parse(string filePath)
    {
        using (var reader = new StreamReader(filePath))
            return Parse(reader);
    }

    public FinantialStatement Parse(MemoryStream stream)
    {
        using (var reader = new StreamReader(stream))
            return Parse(reader);
    }

    private FinantialStatement Parse(StreamReader reader)
    {
        using (var csv = new CsvReader(reader, _config))
        {
            csv.Context.RegisterClassMap<DividendMap>();
            csv.Context.RegisterClassMap<TradeMap>();
            var statement = new FinantialStatement();
            while (csv.Read())
            {
                var tableType = csv.GetField(0);
                var rowType = csv.GetField(1);

                if (rowType == RowType.Header)
                {
                    csv.ReadHeader();
                    continue;
                }

                if (rowType == RowType.Total || rowType == RowType.SubTotal)
                {
                    continue;
                }
                if (rowType == RowType.Data && csv.GetField(2) != RowType.Total)
                {
                    switch (csv.HeaderRecord[0])
                    {
                        case "Dividends":
                            statement.Dividends.Add(csv.GetRecord<Dividend>());
                            break;
                        case "Trades":
                            statement.Trades.Add(csv.GetRecord<Trade>());
                            break;
                        default:
                            break;
                    }
                }
            }
            return statement;
        }
    }

    private static class RowType
    {
        public static string Header => "Header";
        public static string Data => "Data";
        public static string Total => "Total";
        public static string SubTotal => "SubTotal";
    }
}
