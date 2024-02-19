using InvestingTaxesPoc.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Services.AddTransient<IStatementParser, CsvStatementParser>();
builder.Services.AddTransient<IRateService, RateService>();
builder.Services.AddTransient<ITaxCalculationService, TaxCalculationService>();
builder.Services.AddHttpClient<IRateService, RateService>("Bank", httpClient =>
{
    httpClient.BaseAddress = new Uri("https://bank.gov.ua");
});

using IHost host = builder.Build();

await RunAsync(host.Services, args);
host.Run();


static async Task RunAsync(IServiceProvider hostProvider, string[] args)
{
    using IServiceScope serviceScope = hostProvider.CreateScope();
    IServiceProvider provider = serviceScope.ServiceProvider;
    var taxService = provider.GetRequiredService<ITaxCalculationService>();
    var parser = provider.GetRequiredService<IStatementParser>();
    
    var filePath = args.Any() ? args[0] : "example.csv";
    var statement = parser.Parse(filePath);
    var statementTax = await taxService.CalculateTax(statement);

    Console.WriteLine(statement);
    Console.WriteLine("Calculated Taxes");
    Console.WriteLine(statementTax);

    Console.WriteLine();
}
