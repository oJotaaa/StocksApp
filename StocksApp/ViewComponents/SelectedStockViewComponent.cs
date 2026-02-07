using Microsoft.AspNetCore.Mvc;
using StocksApp.ServiceContracts;

namespace StocksApp.ViewComponents
{
    public class SelectedStockViewComponent : ViewComponent
    {
        private readonly IFinnhubService _finnhubService;

        public SelectedStockViewComponent(IFinnhubService finnhubService)
        {
            _finnhubService = finnhubService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string stockSymbol)
        {
            Dictionary<string, object>? companyProfile = await _finnhubService.GetCompanyProfile(stockSymbol);
            Dictionary<string, object>? stockPriceQuote = await _finnhubService.GetStockPriceQuote(stockSymbol);

            if (companyProfile != null && stockPriceQuote != null)
            {
                companyProfile["price"] = stockPriceQuote["c"];
                companyProfile["stockSymbol"] = stockSymbol;
                return View(companyProfile);
            }

            return View();
        }
    }
}
