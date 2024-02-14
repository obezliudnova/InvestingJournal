using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace InvestingTaxesPoc.Services
{
    internal class RateService : IRateService
    {
        const string exchangeServiceUrl = "NBUStatService/v1/statdirectory/exchangenew";
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _options;

        public RateService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };
            _options.Converters.Add(new DateOnlyConverter());
        }

        public async Task<List<CurrencyRate>> GetRatesByDateAsync(DateOnly date)
        {
            using HttpResponseMessage response = await _httpClient.GetAsync($"/{exchangeServiceUrl}?json&date={date:yyyyMMdd}");
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<List<CurrencyRate>>(jsonResponse, _options) ?? [];
        }
    }
}
