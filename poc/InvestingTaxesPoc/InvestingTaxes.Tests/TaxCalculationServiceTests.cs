using FluentAssertions;
using InvestingTaxesPoc.Services;
using NSubstitute;

public partial class MockHttpMessageHandler
{
    [TestFixture]
    public class TaxCalculationServiceTests
    {
        private TaxCalculationService _taxCalculationService;
        private IRateService _rateService;

        [SetUp]
        public void Setup()
        {
            _rateService = Substitute.For<IRateService>();
            _taxCalculationService = new TaxCalculationService(_rateService);
        }

        [Test]
        public async Task CalculateTax_ShouldCalculateCorrectTax()
        {
            // Arrange
            var statement = new FinantialStatement
            {
                Dividends = new List<Dividend>
                {
                    new Dividend { Currency = "USD", Date = new DateTime(2024, 2, 14), Amount = 1000 },
                    new Dividend { Currency = "EUR", Date = new DateTime(2024, 2, 14), Amount = 500 }
                }
            };

            double rate = 0.2;
            var expectedDividendTaxes = new List<DividendTax>
            {
                new DividendTax(statement.Dividends[0], rate),
                new DividendTax (statement.Dividends[0], rate),
            };

            _rateService.GetCurrencyRateAsync(Arg.Any<DateOnly>(), Arg.Any<string>()).Returns(new CurrencyRate { Rate = rate });

            // Act
            var result = await _taxCalculationService.CalculateTax(statement);

            // Assert
            result.DividendTax.Should().BeEquivalentTo(expectedDividendTaxes);
            result.TotalPersonalIncomeTax.Should().Be(300);
            result.MilitaryDutyTax.Should().Be(75);
        }
    }
}

