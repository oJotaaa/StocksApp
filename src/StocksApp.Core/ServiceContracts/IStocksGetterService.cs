using StocksApp.ServiceContracts.DTO;

namespace StocksApp.ServiceContracts
{
    public interface IStocksGetterService
    {
        Task<List<BuyOrderResponse>> GetBuyOrders();
        Task<List<SellOrderResponse>> GetSellOrders();
    }
}
