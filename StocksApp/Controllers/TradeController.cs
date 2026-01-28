using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StocksApp.Services.Interfaces;
using StocksApp.ViewModels;
using System.Text.Json;

namespace StocksApp.Controllers
{
    public class TradeController : Controller
    {
        private readonly IOptions<TradingOptions> _tradingOptions;
        private readonly IFinnhubService _finnhubService;
        private readonly IConfiguration _configuration;

        public TradeController(IOptions<TradingOptions> tradingOptions, IFinnhubService finnhubService, IConfiguration configuration)
        {
            _tradingOptions = tradingOptions;
            _finnhubService = finnhubService;
            _configuration = configuration;
        }

        [Route("/")]
        public async Task<IActionResult> Index()
        {
            string defaultStockSymbol = _tradingOptions.Value.DefaultStockSymbol;
            var companyProfile = await _finnhubService.GetCompanyProfile(defaultStockSymbol);
            var stockPriceQuote = await _finnhubService.GetStockPriceQuote(defaultStockSymbol);

            StockTrade stockTrade = new StockTrade
            {
                StockSymbol = defaultStockSymbol,
                StockName = companyProfile != null && companyProfile.ContainsKey("name") ? ((JsonElement)companyProfile["name"]).GetString() : "N/A",
                Price = stockPriceQuote != null && stockPriceQuote.ContainsKey("c") ? ((JsonElement)stockPriceQuote["c"]).GetDouble() : 0.0,
                Quantity = 0
            };

            ViewBag.ApiKey = _configuration["apiKey"];

            return View(stockTrade);
        }
    }
}
