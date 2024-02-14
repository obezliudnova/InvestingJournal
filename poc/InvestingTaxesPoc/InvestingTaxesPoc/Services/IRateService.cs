namespace InvestingTaxesPoc.Services
{
    internal interface IRateService
    {
        Task<List<CurrencyRate>> GetRatesByDateAsync(DateOnly date);
    }
}