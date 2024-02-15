namespace InvestingTaxesPoc.Services
{
    internal interface IRateService
    {
        Task<List<CurrencyRate>> GetRatesByDateAsync(DateOnly date);
        Task<CurrencyRate> GetCurrencyRateAsync(DateOnly date, string currencyCode);
    }
}
