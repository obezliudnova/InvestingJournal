namespace InvestingTaxesPoc.Services;
internal class TaxCalculationService(IRateService rateService) : ITaxCalculationService
{
    private readonly IRateService _rateService = rateService;

    public async Task<FinantialStatementTaxes> CalculateTax(FinantialStatement statement)
    {
        var dividendTaxes = new List<DividendTax>();
        foreach (var dividend in statement.Dividends)
        {
            var rate = await _rateService.GetCurrencyRateAsync(DateOnly.FromDateTime(dividend.Date), dividend.Currency!);
            dividendTaxes.Add(new DividendTax(dividend, rate.Rate));
        }
        return new FinantialStatementTaxes() { DividendTax = dividendTaxes };
    }
}
