using Microsoft.Extensions.Logging;
using StocksApp.RepositoryContracts;
using StocksApp.ServiceContracts;
using System.Text.Json;

namespace StocksApp.Services
{
    public class FinnhubGetterService : IFinnhubGetterService
    {
        private readonly IFinnhubRepository _finnhubRepository;
        private readonly ILogger<FinnhubGetterService> _logger;

        public FinnhubGetterService(IFinnhubRepository finnhubRepository, ILogger<FinnhubGetterService> logger)
        {
            _finnhubRepository = finnhubRepository;
            _logger = logger;
        }

        public async Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol)
        {
            // Log
            _logger.LogInformation("GetCompanyProfile called from FinnhubService for symbol {StockSymbol}", stockSymbol);

            if (string.IsNullOrEmpty(stockSymbol))
                return null;

            return await _finnhubRepository.GetCompanyProfile(stockSymbol);
        }

        public async Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol)
        {
            // Log
            _logger.LogInformation("GetStockPriceQuote called from FinnhubService for symbol {StockSymbol}", stockSymbol);

            if (string.IsNullOrEmpty(stockSymbol))
                return null;

            return await _finnhubRepository.GetStockPriceQuote(stockSymbol);
        }

        public async Task<List<Dictionary<string, string>>?> GetStocks()
        {
            // Log
            _logger.LogInformation("GetStocks called from FinnhubService");

            return await _finnhubRepository.GetStocks();
        }
    }
}
