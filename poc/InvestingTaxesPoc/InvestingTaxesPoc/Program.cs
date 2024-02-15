using InvestingTaxesPoc.Services;

var filePath = args.Any() ? args[0] : "example.csv";
var parser = new CsvStatementParser();
var statement = parser.Parse(filePath);

using HttpClient httpClient = new()
{
    BaseAddress = new Uri("https://bank.gov.ua"),
};

IRateService rateService = new RateService(httpClient);
ITaxCalculationService taxService = new TaxCalculationService(rateService);
var statementTax = await taxService.CalculateTax(statement);

Console.WriteLine(statement);
Console.WriteLine("Calculated Taxes");
Console.WriteLine(statementTax);

Console.ReadLine();
