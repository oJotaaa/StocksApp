using StocksApp.Entities;
using StocksApp.ServiceContracts.DTO;

namespace StocksApp.ServiceContracts.Extensions
{
    public static class SellOrderRequestExtensions
    {
        public static SellOrder ToSellOrder(this SellOrderRequest sellOrderRequest)
        {
            return new SellOrder
            {
                StockSymbol = sellOrderRequest.StockSymbol,
                StockName = sellOrderRequest.StockName,
                DateAndTimeOfOrder = sellOrderRequest.DateAndTimeOfOrder,
                Quantity = sellOrderRequest.Quantity,
                Price = sellOrderRequest.Price
            };
        }
    }
}
