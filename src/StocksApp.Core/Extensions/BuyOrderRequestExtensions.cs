using StocksApp.Entities;
using StocksApp.ServiceContracts.DTO;

namespace StocksApp.ServiceContracts.Extensions
{
    public static class BuyOrderRequestExtensions
    {
        public static BuyOrder ToBuyOrder(this BuyOrderRequest buyOrderRequest)
        {
            return new BuyOrder
            {
                StockSymbol = buyOrderRequest.StockSymbol,
                StockName = buyOrderRequest.StockName,
                DateAndTimeOfOrder = buyOrderRequest.DateAndTimeOfOrder,
                Quantity = buyOrderRequest.Quantity,
                Price = buyOrderRequest.Price
            };
        }
    }
}
