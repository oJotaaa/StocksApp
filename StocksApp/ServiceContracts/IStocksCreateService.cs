using StocksApp.ServiceContracts.DTO;

namespace StocksApp.ServiceContracts
{
    public interface IStocksCreateService
    {
        Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest);
        Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest);
    }
}
