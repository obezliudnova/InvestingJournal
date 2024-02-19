using System.Net;
using System.Text.Json;
using FluentAssertions;
using InvestingTaxesPoc.Services;

namespace InvestingTaxesPoc.Tests
{
    [TestFixture]
    public class RateServiceTests
    {
        private HttpClient? _httpClient;
        private JsonSerializerOptions _options;

        [SetUp]
        public void SetUp()
        {
            _options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };
            _options.Converters.Add(new DateOnlyConverter());
        }

        [Test]
        public async Task GetRatesByDateAsync_ShouldDeserializeJsonResponse()
        {
            // Arrange

            var date = new DateOnly(2024, 2, 14);
            var expectedRates = new List<CurrencyRate>
            {
                new CurrencyRate { Code = "USD", Rate = 1.0, ExchangeDate = date },
                new CurrencyRate { Code = "EUR", Rate = 1.2, ExchangeDate = date }
            };

            var jsonResponse = JsonSerializer.Serialize(expectedRates, _options);
            RateService sut = CreateSut(jsonResponse);

            // Act
            var rates = await sut.GetRatesByDateAsync(date);

            // Assert
            rates.Should().BeEquivalentTo(expectedRates);
        }

        private RateService CreateSut(string jsonResponse)
        {
            var mockHttpMessageHandler = new MockHttpMessageHandler(jsonResponse);
            _httpClient = new HttpClient(mockHttpMessageHandler)
            {
                BaseAddress = new Uri("https://test"),
            };
            var sut = new RateService(_httpClient);
            return sut;
        }

        [Test]
        public async Task GetCurrencyRateAsync_ShouldReturnFilteredCurrencyRate()
        {
            // Arrange
            var date = new DateOnly(2024, 2, 14);
            var expectedRates = new List<CurrencyRate>
            {
                new CurrencyRate { Code = "USD", Rate = 1.0, ExchangeDate = date },
                new CurrencyRate { Code = "EUR", Rate = 1.2, ExchangeDate = date }
            };

            var jsonResponse = JsonSerializer.Serialize(expectedRates, _options);
            RateService sut = CreateSut(jsonResponse);

            // Act
            var rate = await sut.GetCurrencyRateAsync(date, "EUR");

            // Assert
            rate.Should().BeEquivalentTo(expectedRates[1]);
        }

        [TearDown]
        public void Cleanup()
        {
            _httpClient?.Dispose();
        }
    }
}

public partial class MockHttpMessageHandler : HttpMessageHandler
{
    private readonly string _jsonResponse;

    public MockHttpMessageHandler(string jsonResponse)
    {
        _jsonResponse = jsonResponse;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return Task.FromResult(MockSend(request, cancellationToken));
    }

    protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return MockSend(request, cancellationToken);
    }

    public virtual HttpResponseMessage MockSend(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(_jsonResponse?? "")
        };
    }
}
