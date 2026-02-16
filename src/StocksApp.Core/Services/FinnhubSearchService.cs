using Microsoft.Extensions.Logging;
using StocksApp.RepositoryContracts;
using StocksApp.ServiceContracts;
using System.Text.Json;

namespace StocksApp.Services
{
    public class FinnhubSearchService : IFinnhubSearchService
    {
        private readonly IFinnhubRepository _finnhubRepository;
        private readonly ILogger<FinnhubSearchService> _logger;

        public FinnhubSearchService(IFinnhubRepository finnhubRepository, ILogger<FinnhubSearchService> logger)
        {
            _finnhubRepository = finnhubRepository;
            _logger = logger;
        }
        public async Task<Dictionary<string, object>?> SearchStocks(string stockSymbolToSearch)
        {
            // Log
            _logger.LogInformation("SearchStocks called from FinnhubService for symbolToSearch {StockSymbol}", stockSymbolToSearch);

            if (string.IsNullOrEmpty(stockSymbolToSearch))
                return null;

            return await _finnhubRepository.SearchStocks(stockSymbolToSearch);
        }
    }
}
