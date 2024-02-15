using InvestingTaxesPoc.Services;

var filePath = args.Any() ? args[0] : "example.csv";
var parser = new CsvStatementParser();
var statement = parser.Parse(filePath);

using HttpClient httpClient = new()
{
    BaseAddress = new Uri("https://bank.gov.ua"),
};

IRateService rateService = new RateService(httpClient);
var dates = new List<DateOnly>();
statement.Dividends.ForEach(d => dates.Add(DateOnly.FromDateTime(d.Date)));
var rates = new Dictionary<DateOnly, List<CurrencyRate>>();
foreach (var date in dates.Distinct())
{
    var dateRates = await rateService.GetRatesByDateAsync(date);
    rates[date] = dateRates;
}

var dividendTaxes = new List<DividendTax>();
foreach (var dividend in statement.Dividends)
{
    var rate = await rateService.GetCurrencyRateAsync(DateOnly.FromDateTime(dividend.Date), dividend.Currency!);
    dividendTaxes.Add(new DividendTax(dividend, rate.Rate));
}
var statementTax = new FinantialStatementTaxes() { DividendTax = dividendTaxes };
Console.WriteLine(statement);
Console.WriteLine("Calculated Taxes");
Console.WriteLine(statementTax);

Console.ReadLine();
