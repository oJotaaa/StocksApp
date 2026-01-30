using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StocksApp.ServiceContracts;
using StocksApp.ViewModels;
using System.Text.Json;

namespace StocksApp.Controllers
{
    public class TradeController : Controller
    {
        private readonly TradingOptions _tradingOptions;
        private readonly IConfiguration _configuration;
        private readonly IFinnhubService _finnhubService;
        private readonly IStocksService _stocksService;

        public TradeController(IOptions<TradingOptions> tradingOptions, IFinnhubService finnhubService, IConfiguration configuration, IStocksService stocksService)
        {
            _tradingOptions = tradingOptions.Value;
            _finnhubService = finnhubService;
            _configuration = configuration;
            _stocksService = stocksService;
        }

        [Route("/")]
        [Route("[action]")]
        [Route("~/[controller]")]
        public async Task<IActionResult> Index()
        {
            if (string.IsNullOrEmpty(_tradingOptions.DefaultStockSymbol))
                _tradingOptions.DefaultStockSymbol = "MSFT";

            string? defaultStockSymbol = _tradingOptions.DefaultStockSymbol;
            var companyProfile = await _finnhubService.GetCompanyProfile(defaultStockSymbol!);
            var stockPriceQuote = await _finnhubService.GetStockPriceQuote(defaultStockSymbol!);

            StockTrade stockTrade = new StockTrade() { StockSymbol = defaultStockSymbol };

            if (companyProfile != null && stockPriceQuote != null)
            {
                stockTrade = new StockTrade()
                {
                    StockSymbol = defaultStockSymbol,
                    StockName = companyProfile != null && companyProfile.ContainsKey("name") ? ((JsonElement)companyProfile["name"]).GetString() : "N/A",
                    Price = stockPriceQuote != null && stockPriceQuote.ContainsKey("c") ? ((JsonElement)stockPriceQuote["c"]).GetDouble() : 0.0,
                    Quantity = 0
                };
            }

            ViewBag.ApiKey = _configuration["apiKey"];

            return View(stockTrade);
        }
    }
}
