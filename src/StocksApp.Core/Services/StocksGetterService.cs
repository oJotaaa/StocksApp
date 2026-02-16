using Microsoft.Extensions.Logging;
using StocksApp.Entities;
using StocksApp.RepositoryContracts;
using StocksApp.ServiceContracts;
using StocksApp.ServiceContracts.DTO;
using StocksApp.ServiceContracts.Extensions;
using StocksApp.Services.Helpers;

namespace StocksApp.Services
{
    public class StocksGetterService : IStocksGetterService
    {
        // Private fields
        private readonly IStocksRepository _stocksRepository;
        private readonly ILogger<StocksCreateService> _logger;

        // Constructor
        public StocksGetterService(IStocksRepository stocksRepository, ILogger<StocksCreateService> logger) 
        {
            _stocksRepository = stocksRepository;
            _logger = logger;
        }

        public async Task<List<BuyOrderResponse>> GetBuyOrders()
        {
            // Log
            _logger.LogInformation("GetBuyOrders called from StocksService");

            List<BuyOrder> buyOrders = await _stocksRepository.GetBuyOrders();
            return buyOrders.Select(temp => temp.ToBuyOrderResponse()).ToList();
        }

        public async Task<List<SellOrderResponse>> GetSellOrders()
        {
            // Log
            _logger.LogInformation("GetSellOrders called from StocksService");

            List<SellOrder> sellOrders = await _stocksRepository.GetSellOrders();

            return sellOrders.Select(temp => temp.ToSellOrderResponse()).ToList();
        }
    }
}
