
namespace InvestingTaxesPoc.Services;

internal interface ITaxCalculationService
{
    Task<FinantialStatementTaxes> CalculateTax(FinantialStatement statement);
}
