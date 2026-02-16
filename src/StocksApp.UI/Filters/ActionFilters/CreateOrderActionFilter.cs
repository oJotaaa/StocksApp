using Microsoft.AspNetCore.Mvc.Filters;
using StocksApp.Controllers;
using StocksApp.Entities;
using StocksApp.ServiceContracts;
using StocksApp.ViewModels;
using System.Diagnostics;

namespace StocksApp.Filters.ActionFilters
{
    public class CreateOrderActionFilter : IAsyncActionFilter
    {
        private readonly IConfiguration _configuration;

        public CreateOrderActionFilter(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.Controller is TradeController)
            {
                TradeController controller = (TradeController)context.Controller;
                var orderRequestObject = context.ActionArguments["orderRequest"];

                if (orderRequestObject != null)
                {
                    IOrderRequest orderRequest = (IOrderRequest)orderRequestObject;
                    if (!controller.ModelState.IsValid)
                    {
                        controller.ViewBag.Errors = controller.ModelState.Values.SelectMany(temp => temp.Errors).Select(temp => temp.ErrorMessage).ToList();
                        controller.ViewBag.ApiKey = _configuration["apiKey"];
                        StockTrade stockTrade = new StockTrade()
                        {
                            StockSymbol = orderRequest.StockSymbol,
                            StockName = orderRequest.StockName,
                            Price = orderRequest.Price,
                            Quantity = orderRequest.Quantity,
                        };
                        context.Result = controller.View("Index", stockTrade);
                        return;
                    }
                }
            }
            await next();
        }
    }
}
