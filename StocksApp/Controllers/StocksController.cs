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
        private readonly IFinnhubService _finnhubService;
        private readonly IStocksService _stocksService;

        public StocksController(IOptions<TradingOptions> tradingOptions, IFinnhubService finnhubService, IStocksService stocksService)
        {
            _tradingOptions = tradingOptions.Value;
            _finnhubService = finnhubService;
            _stocksService = stocksService;
        }

        [HttpGet]
        [Route("[action]/{stock?}")]
        public async Task<IActionResult> Explore(string stock)
        {
            ViewBag.Stock = stock;
            string? topPopularStocksString = _tradingOptions.Top25PopularStocks;
            string[] topPopularStocks = topPopularStocksString!.Split(",");

            List<Dictionary<string, string>>? stocksFromGetStocks = await _finnhubService.GetStocks();

            List<Stock> stocks = stocksFromGetStocks!.Where(stock => topPopularStocks.Contains(stock["symbol"])).Select(stock => new Stock() { StockName = stock["description"], StockSymbol = stock["symbol"] }).ToList();
            return View(stocks);
        }
    }
}
