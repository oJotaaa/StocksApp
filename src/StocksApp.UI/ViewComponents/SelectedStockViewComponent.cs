using Microsoft.AspNetCore.Mvc;
using StocksApp.ServiceContracts;

namespace StocksApp.ViewComponents
{
    public class SelectedStockViewComponent : ViewComponent
    {
        private readonly IFinnhubGetterService _finnhubGetterService;

        public SelectedStockViewComponent(IFinnhubGetterService finnhubGetterService)
        {
            _finnhubGetterService = finnhubGetterService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string stockSymbol)
        {
            Dictionary<string, object>? companyProfile = await _finnhubGetterService.GetCompanyProfile(stockSymbol);
            Dictionary<string, object>? stockPriceQuote = await _finnhubGetterService.GetStockPriceQuote(stockSymbol);

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
