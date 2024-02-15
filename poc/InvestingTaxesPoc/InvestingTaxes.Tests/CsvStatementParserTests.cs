using FluentAssertions;
using System.Text;

namespace InvestingTaxes.Tests;
public class CsvStatementParserTests
{
    private CsvStatementParser _sut;

    [SetUp]
    public void Setup()
    {
        _sut = new CsvStatementParser();
    }

    [Test]
    public void Parse_ReturnsFinancialStatementWithDividends_WhenValidDataProvided()
    {
        // Arrange
        var csvContent = "Statement,Header,Field Name, Field Value\n" +
                        "Statement,Data,BrokerName,Interactive Brokers LLC\n" +
                        "Dividends,Header,Currency,Date,Description,Amount\n" +
                        "Dividends,Data,USD,2023-11-06,BND,0.61\n" +
                        "Dividends,Data,USD,2023-12-21,XLF,1.84\n" +
                        "Dividends,Data,Total,,,26.06\n" +
                        "Withholding Tax,Header,Currency,Date,Description,Amount,Code\n" +
                        "Withholding Tax,Data,USD,2023-11-06,BND US Tax,-0.09,\n";

        var expectedDividends = new List<Dividend>
        {
            new Dividend
            {
                Currency = "USD",
                Date = new DateTime(2023, 11, 6),
                Description = "BND",
                Amount = 0.61
            },
            new Dividend
            {
                Currency = "USD",
                Date = new DateTime(2023, 12, 21),
                Description = "XLF",
                Amount = 1.84
            }
        };

        var expectedStatement = new FinantialStatement
        {
            Dividends = expectedDividends,
            Trades = new List<Trade>()
        };

        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(csvContent));

        // Act
        var result = _sut.Parse(stream);

        // Assert
        result.Should().BeEquivalentTo(expectedStatement);
    }

    [Test]
    public void Parse_ReturnsFinancialStatementWithTrades_WhenValidDataProvided()
    {
        // Arrange
        var csvContent = "Trades,Header,DataDiscriminator,Asset Category,Currency,Symbol,Date/Time,Quantity,T. Price,C. Price,Proceeds,Comm/Fee,Basis,Realized P/L,MTM P/L,Code\n" +
                        @"Trades,Data,Order,Stocks,EUR,AAA,""2023 - 11 - 08, 03:00:26"",1,596.8,600,-596.8,-1.496696,598.296696,0,3.2,O" + "\n" +
                        "Trades,SubTotal,,Stocks,EUR,AAA,,1,,,-596.8,-1.496696,598.296696,0,3.2,\n" +
                        "Trades,Total,,Stocks,EUR,,,,,,-596.8,-1.496696,598.296696,0,3.2, \n" +
                        @"Trades,Data,Order,Stocks,USD,BND,""2023 - 10 - 09, 09:30:02"",3,69.14,69.48,-207.42,-0.35535725,207.77535725,0,1.02,O" + "\n" +
                        "Trades,Header,DataDiscriminator,Asset Category,Currency,Symbol,Date/Time,Quantity,T. Price,,Proceeds,Comm in USD,,,MTM in USD,Code\n" +
                        @"Trades,Data,Order,Forex,USD,EUR.USD,""2023 - 10 - 08, 17:15:00"",-500,1.0553,,527.65,-2,,,-0.7," + "\n" +
                        "Trades,SubTotal,,Forex,USD,EUR.USD,,-6242.37,,,6657.630291,-12,,,-13.630638,\n" +
                        "Trades,Total,,Forex,USD,,,,,,6657.630291,-12,,,-13.630638, \n";

        var expectedTrades = new List<Trade>
        {
            new Trade
            {
                AssetCategory = "Stocks",
                Currency = "EUR",
                Symbol = "AAA",
                DateTime = new DateTime(2023, 11, 8, 3, 0, 26),
                Quantity = 1,
                TPrice = 596.8,
                Proceeds = -596.8,
                CommUSD = -1.496696,
                Basis = 598.296696,
                RealizedPL = 0,
                MTMPL = 3.2,
                Code = "O"
            },
            new Trade
            {
                AssetCategory = "Stocks",
                Currency = "USD",
                Symbol = "BND",
                DateTime = new DateTime(2023, 10, 9, 9, 30, 2),
                Quantity = 3,
                TPrice = 69.14,
                Proceeds = -207.42,
                CommUSD = -0.35535725,
                Basis = 207.77535725,
                RealizedPL = 0,
                MTMPL = 1.02,
                Code = "O"
            },
            new Trade
            {
                AssetCategory = "Forex",
                Currency = "USD",
                Symbol = "EUR.USD",
                DateTime = new DateTime(2023, 10, 8, 17, 15, 0),
                Quantity = -500,
                TPrice = 1.0553,
                Proceeds = 527.65,
                CommUSD = -2,
                Basis = null,
                RealizedPL = null,
                MTMPL = -0.7,
                Code = ""
            }
        };

        var expectedStatement = new FinantialStatement
        {
            Dividends = new List<Dividend>(),
            Trades = expectedTrades
        };

        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(csvContent));

        // Act
        var result = _sut.Parse(stream);

        // Assert
        result.Should().BeEquivalentTo(expectedStatement);
    }
}
