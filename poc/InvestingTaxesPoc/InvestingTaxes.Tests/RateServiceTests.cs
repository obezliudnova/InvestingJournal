using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using InvestingTaxesPoc.Services;
using NSubstitute;
using NUnit.Framework;

namespace InvestingTaxesPoc.Tests
{
    [TestFixture]
    public class RateServiceTests
    {
        private RateService _rateService;
        private HttpClient _httpClient;

        [SetUp]
        public void Setup()
        {
            var mockHttpMessageHandler = Substitute.ForPartsOf<MockHttpMessageHandler>();

            _httpClient = new HttpClient(mockHttpMessageHandler)
            {
                BaseAddress = new Uri("https://test"),
            };
            _rateService = new RateService(_httpClient);
        }

        [Test]
        public async Task GetRatesByDateAsync_ShouldDeserializeJsonResponse()
        {
            // Arrange
            var date = new DateOnly(2024, 2, 14);
            var expectedRates = new List<CurrencyRate>
            {
                new CurrencyRate { Code = "USD", Rate = 1.0 },
                new CurrencyRate { Code = "EUR", Rate = 1.2 }
            };
            var jsonResponse = JsonSerializer.Serialize(expectedRates);

            _httpClient.GetAsync(Arg.Any<string>()).Returns(new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent(jsonResponse)
            });

            // Act
            var rates = await _rateService.GetRatesByDateAsync(date);

            // Assert
            rates.Should().BeEquivalentTo(expectedRates);
        }

        [Test]
        public async Task GetCurrencyRateAsync_ShouldReturnCurrencyRate()
        {
            // Arrange
            var date = new DateOnly(2024, 2, 14);
            var expectedRates = new List<CurrencyRate>
            {
                new CurrencyRate { Code = "USD", Rate = 1.0 },
                new CurrencyRate { Code = "EUR", Rate = 1.2 }
            };

            _httpClient.GetAsync(Arg.Any<string>()).Returns(new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(expectedRates))
            });

            // Act
            var rate = await _rateService.GetCurrencyRateAsync(date, "EUR");

            // Assert
            rate.Should().BeEquivalentTo(expectedRates[1]);
        }

        [TearDown]
        public void Cleanup()
        {
            _httpClient.Dispose();
        }
    }
}

public class MockHttpMessageHandler : HttpMessageHandler
{
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
        throw new NotImplementedException();
    }
}
