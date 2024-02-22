using System.Globalization;
using CsvHelper;

namespace InvestingTaxesPoc.Services;
internal class CsvStatementWriter
{
    public void WriteStatementToFile(FinantialStatementTaxes taxes)
    {
        var taxesRows = taxes?.DividendTax?
            .Select(d => new { 
                d.Dividend!.Currency,
                Date = d.Dividend!.Date.ToShortDateString(),
                d.Dividend!.Description,
                d.Dividend!.Amount,
                d.CurrencyRate,
                d.PersonalIncomeTax,
                d.MilitaryDutyTax 
            })
            .ToList() ?? [];
        taxesRows.Add(new { 
            Currency = "", 
            Date = "", 
            Description = "TOTAL", 
            Amount = 0.0, 
            CurrencyRate = 0.0, 
            PersonalIncomeTax = taxes!.TotalPersonalIncomeTax, 
            MilitaryDutyTax = taxes!.TotalMilitaryDutyTax 
        });
        
        using var writer = new StreamWriter("taxes.csv");
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        csv.WriteRecords(taxesRows);
    }
}
