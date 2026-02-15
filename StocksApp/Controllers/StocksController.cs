using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StocksApp.ServiceContracts;
using StocksApp.ViewModels;

namespace StocksApp.Controllers
{
    [Route("[controller]")]
    public class StocksController : Controller
    {
        private readonly TradingOptions _tradingOptions;
        private readonly ILogger<StocksController> _logger;
        private readonly IFinnhubGetterService _finnhubGetterService;

        public StocksController(IOptions<TradingOptions> tradingOptions, IFinnhubGetterService finnhubGetterService, ILogger<StocksController> logger)
        {
            _tradingOptions = tradingOptions.Value;
            _finnhubGetterService = finnhubGetterService;
            _logger = logger;
        }

        [HttpGet]
        [Route("[action]/{stock?}")]
        public async Task<IActionResult> Explore(string? stock)
        {
            _logger.LogInformation("Explore action called from StocksController");

            ViewBag.Stock = stock;
            string? topPopularStocksString = _tradingOptions.Top25PopularStocks;
            string[] topPopularStocks = topPopularStocksString!.Split(",");

            List<Dictionary<string, string>>? stocksFromGetStocks = await _finnhubGetterService.GetStocks();

            List<Stock> stocks = stocksFromGetStocks!.Where(stock => topPopularStocks.Contains(stock["symbol"])).Select(stock => new Stock() { StockName = stock["description"], StockSymbol = stock["symbol"] }).ToList();
            return View("Explore", stocks);
        }
    }
}
