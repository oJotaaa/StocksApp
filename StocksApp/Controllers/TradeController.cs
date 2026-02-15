using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Rotativa.AspNetCore;
using StocksApp.Entities;
using StocksApp.Filters.ActionFilters;
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
        private readonly IFinnhubGetterService _finnhubGetterService;
        private readonly IStocksGetterService _stocksGetterService;
        private readonly IStocksCreateService _stocksCreateService;

        public TradeController(IOptions<TradingOptions> tradingOptions, IFinnhubGetterService finnhubGetterService, IConfiguration configuration, IStocksCreateService stocksCreateService, IStocksGetterService stocksGetterService, ILogger<TradeController> logger)
        {
            _logger = logger;
            _configuration = configuration;
            _tradingOptions = tradingOptions.Value;
            _finnhubGetterService = finnhubGetterService;
            _stocksCreateService = stocksCreateService;
            _stocksGetterService = stocksGetterService;
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
            var companyProfile = await _finnhubGetterService.GetCompanyProfile(selectedStockSymbol!);
            var stockPriceQuote = await _finnhubGetterService.GetStockPriceQuote(selectedStockSymbol!);

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
        [TypeFilter(typeof(CreateOrderActionFilter))]
        public async Task<IActionResult> BuyOrder(BuyOrderRequest orderRequest)
        {
            // Log
            _logger.LogInformation("BuyOrder action called from TradeController with buyOrder: {@buyOrder}", orderRequest);

            orderRequest.DateAndTimeOfOrder = DateTime.Now;
            BuyOrderResponse buyOrderResponse = await _stocksCreateService.CreateBuyOrder(orderRequest);

            return RedirectToAction("Orders", "Trade");
        }

        [HttpPost]
        [Route("[action]")]
        [TypeFilter(typeof(CreateOrderActionFilter))]
        public async Task<IActionResult> SellOrder(SellOrderRequest orderRequest)
        {
            // Log
            _logger.LogInformation("SellOrder action called from TradeController with sellOrder: {@sellOrder}", orderRequest);

            orderRequest.DateAndTimeOfOrder = DateTime.Now;
            SellOrderResponse sellOrderResponse = await _stocksCreateService.CreateSellOrder(orderRequest);

            return RedirectToAction("Orders", "Trade");
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Orders()
        {
            // Log
            _logger.LogInformation("Orders action called from TradeController");

            List<BuyOrderResponse> buyOrders = await _stocksGetterService.GetBuyOrders();
            List<SellOrderResponse> sellOrders = await _stocksGetterService.GetSellOrders();
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

            List<BuyOrderResponse> buyOrders = await _stocksGetterService.GetBuyOrders();
            List<SellOrderResponse> sellOrders = await _stocksGetterService.GetSellOrders();
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
