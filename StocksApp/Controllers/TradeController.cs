using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Rotativa.AspNetCore;
using StocksApp.Entities;
using StocksApp.ServiceContracts;
using StocksApp.ServiceContracts.DTO;
using StocksApp.ViewModels;
using System.Text.Json;

namespace StocksApp.Controllers
{
    [Route("[controller]")]
    public class TradeController : Controller
    {
        private readonly TradingOptions _tradingOptions;
        private readonly IConfiguration _configuration;
        private readonly ILogger<TradeController> _logger;
        private readonly IFinnhubService _finnhubService;
        private readonly IStocksService _stocksService;

        public TradeController(IOptions<TradingOptions> tradingOptions, IFinnhubService finnhubService, IConfiguration configuration, IStocksService stocksService, ILogger<TradeController> logger)
        {
            _tradingOptions = tradingOptions.Value;
            _finnhubService = finnhubService;
            _configuration = configuration;
            _stocksService = stocksService;
            _logger = logger;
        }

        [Route("/")]
        [Route("[action]/{stockSymbol?}")]
        [Route("~/[controller]")]
        public async Task<IActionResult> Index(string? stockSymbol)
        {
            // Log
            _logger.LogInformation("Index action called from TradeController with stockSymbol: {stockSymbol}", stockSymbol);

            if (string.IsNullOrEmpty(_tradingOptions.DefaultStockSymbol))
                _tradingOptions.DefaultStockSymbol = "MSFT";

            string? selectedStockSymbol;
            if (string.IsNullOrEmpty(stockSymbol)) 
                selectedStockSymbol = _tradingOptions.DefaultStockSymbol;
            else
                selectedStockSymbol = stockSymbol;

            uint defaultOrderQuantity = _tradingOptions.DefaultOrderQuantity;
            var companyProfile = await _finnhubService.GetCompanyProfile(selectedStockSymbol!);
            var stockPriceQuote = await _finnhubService.GetStockPriceQuote(selectedStockSymbol!);

            StockTrade stockTrade = new StockTrade() { StockSymbol = selectedStockSymbol };

            if (companyProfile != null && stockPriceQuote != null)
            {
                stockTrade = new StockTrade()
                {
                    StockSymbol = selectedStockSymbol,
                    StockName = companyProfile != null && companyProfile.ContainsKey("name") ? ((JsonElement)companyProfile["name"]).GetString() : "N/A",
                    Price = stockPriceQuote != null && stockPriceQuote.ContainsKey("c") ? ((JsonElement)stockPriceQuote["c"]).GetDouble() : 0.0,
                    Quantity = defaultOrderQuantity
                };
            }

            ViewBag.ApiKey = _configuration["apiKey"];

            return View(stockTrade);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> BuyOrder(BuyOrderRequest buyOrder)
        {
            // Log
            _logger.LogInformation("BuyOrder action called from TradeController with buyOrder: {@buyOrder}", buyOrder);

            if (!ModelState.IsValid) 
            {
                ViewBag.ApiKey = _configuration["apiKey"];
                StockTrade stockTrade = new StockTrade()
                {
                    StockSymbol = buyOrder.StockSymbol,
                    StockName = buyOrder.StockName,
                    Price = buyOrder.Price,
                    Quantity = buyOrder.Quantity,
                };
                return View("Index", stockTrade);
            }

            buyOrder.DateAndTimeOfOrder = DateTime.Now;
            BuyOrderResponse buyOrderResponse = await _stocksService.CreateBuyOrder(buyOrder);

            return RedirectToAction("Orders", "Trade");
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> SellOrder(SellOrderRequest sellOrder)
        {
            // Log
            _logger.LogInformation("SellOrder action called from TradeController with sellOrder: {@sellOrder}", sellOrder);

            if (!ModelState.IsValid)
            {
                ViewBag.ApiKey = _configuration["apiKey"];
                StockTrade stockTrade = new StockTrade()
                {
                    StockSymbol = sellOrder.StockSymbol,
                    StockName = sellOrder.StockName,
                    Price = sellOrder.Price,
                    Quantity = sellOrder.Quantity,
                };
                return View("Index", stockTrade);
            }

            sellOrder.DateAndTimeOfOrder = DateTime.Now;
            SellOrderResponse sellOrderResponse = await _stocksService.CreateSellOrder(sellOrder);

            return RedirectToAction("Orders", "Trade");
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Orders()
        {
            // Log
            _logger.LogInformation("Orders action called from TradeController");

            List<BuyOrderResponse> buyOrders = await _stocksService.GetBuyOrders();
            List<SellOrderResponse> sellOrders = await _stocksService.GetSellOrders();
            Orders orders = new Orders()
            {
                BuyOrders = buyOrders,
                SellOrders = sellOrders
            };

            return View(orders);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> OrdersPDF()
        {
            // Log
            _logger.LogInformation("OrdersPDF action called from TradeController");

            List<BuyOrderResponse> buyOrders = await _stocksService.GetBuyOrders();
            List<SellOrderResponse> sellOrders = await _stocksService.GetSellOrders();
            Orders orders = new Orders()
            {
                BuyOrders = buyOrders,
                SellOrders = sellOrders
            };

            return new ViewAsPdf("OrdersPDF", orders, ViewData)
            {
                PageMargins = new Rotativa.AspNetCore.Options.Margins()
                {
                    Top = 20,
                    Right = 20,
                    Bottom = 20,
                    Left = 20
                },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
            };
        }
    }
}
