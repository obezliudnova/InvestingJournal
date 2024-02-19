using FluentAssertions;
using InvestingTaxesPoc.Services;
using NSubstitute;

namespace InvestingTaxesPoc.Tests;

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
        [TestCase(1000, 0.2, 36, 6)]
        [TestCase(500, 0.2, 18, 3)]
        [TestCase(0, 0.2, 0, 0)]
        [TestCase(100, 10, 180, 30)]
        public async Task CalculateTax_ShouldCalculateCorrectTax(double amount, double rate, double expectedTotalPersonalIncomeTax, double expectedMilitaryDutyTax)
        {
            // Arrange
            var statement = new FinantialStatement
            {
                Dividends = new List<Dividend>
                {
                    new Dividend { Currency = "USD", Date = new DateTime(2024, 2, 14), Amount = amount },
                    new Dividend { Currency = "EUR", Date = new DateTime(2024, 2, 14), Amount = amount }
                }
            };

            var expectedDividendTaxes = new List<DividendTax>
            {
                new DividendTax(statement.Dividends[0], rate),
                new DividendTax (statement.Dividends[1], rate), //1.5
            };

            _rateService.GetCurrencyRateAsync(Arg.Any<DateOnly>(), Arg.Any<string>()).Returns(new CurrencyRate { Rate = rate });

            // Act
            var result = await _taxCalculationService.CalculateTax(statement);

            // Assert
            result.DividendTax.Should().BeEquivalentTo(expectedDividendTaxes);
            result.TotalPersonalIncomeTax.Should().Be(expectedTotalPersonalIncomeTax);
            result.MilitaryDutyTax.Should().Be(expectedMilitaryDutyTax);
        }
    }
}

