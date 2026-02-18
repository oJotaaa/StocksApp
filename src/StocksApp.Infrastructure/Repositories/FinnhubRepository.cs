using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StocksApp.RepositoryContracts;
using System.Text.Json;

namespace StocksApp.Repositories
{
    public class FinnhubRepository : IFinnhubRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger<FinnhubRepository> _logger;

        public FinnhubRepository(IHttpClientFactory httpClientFactory, IConfiguration configuration, ILogger<FinnhubRepository> logger)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol)
        {
            HttpClient httpClient = _httpClientFactory.CreateClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"https://finnhub.io/api/v1/stock/profile2?symbol={stockSymbol}&token={_configuration["apiKey"]}");

            HttpResponseMessage response = await httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var companyProfile = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonResponse, options);
                return companyProfile;
            }
            else
            {
                string errorResponse = await response.Content.ReadAsStringAsync();
                _logger.LogError("Error fetching company profile for symbol {StockSymbol}: {ErrorResponse}", stockSymbol, errorResponse);
                return null;
            }
        }

        public async Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol)
        {
            HttpClient httpClient = _httpClientFactory.CreateClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"https://finnhub.io/api/v1/quote?symbol={stockSymbol}&token={_configuration["apiKey"]}");

            HttpResponseMessage response = await httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var stockQuote = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonResponse, options);
                return stockQuote;
            }
            else
            {
                string errorResponse = await response.Content.ReadAsStringAsync();
                _logger.LogError("Error fetching stock price quote for symbol {StockSymbol}: {ErrorResponse}", stockSymbol, errorResponse);
                return null;
            }
        }

        public async Task<List<Dictionary<string, string>>?> GetStocks()
        {
            HttpClient httpClient = _httpClientFactory.CreateClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"https://finnhub.io/api/v1/stock/symbol?exchange=US&token={_configuration["apiKey"]}");

            HttpResponseMessage response = await httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var stocksList = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(jsonResponse, options);
                return stocksList;
            }
            else
            {
                string errorResponse = await response.Content.ReadAsStringAsync();
                _logger.LogError("Error fetching stocks list: {ErrorResponse}", errorResponse);
                return null;
            }
        }

        public async Task<Dictionary<string, object>?> SearchStocks(string stockSymbolToSearch)
        {
            HttpClient httpClient = _httpClientFactory.CreateClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"https://finnhub.io/api/v1/search?q={stockSymbolToSearch}&token={_configuration["apiKey"]}");

            HttpResponseMessage response = await httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var searchResults = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonResponse, options);
                return searchResults;
            }
            else
            {
                string errorResponse = await response.Content.ReadAsStringAsync();
                _logger.LogError("Error searching stocks for query {StockSymbolToSearch}: {ErrorResponse}", stockSymbolToSearch, errorResponse);
                return null;
            }
        }
    }
}
