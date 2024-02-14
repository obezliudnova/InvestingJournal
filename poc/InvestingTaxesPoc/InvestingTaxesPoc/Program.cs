using InvestingTaxesPoc.Services;

using HttpClient httpClient = new()
{
    BaseAddress = new Uri("https://bank.gov.ua"),
};

IRateService rateService = new RateService(httpClient);
var dateStrings = new string[] {
"11/6/2023",
"12/6/2023",
"12/21/2023",
"12/26/2023",
"12/28/2023",
};

var dates = (from date in dateStrings.Distinct()
                select DateOnly.Parse(date)).ToList();

var rates = new List<CurrencyRate>();
foreach (var date in dates)
{
    var dateRates = await rateService.GetRatesByDateAsync(date);
    rates.Add(dateRates.Where(r => r.Code == "USD").Single());
}

DumpRates(rates);

static void DumpRates(List<CurrencyRate> rates)
{
    foreach (var rate in rates)
    {
        Console.WriteLine(rate.ToString());
    }
}
