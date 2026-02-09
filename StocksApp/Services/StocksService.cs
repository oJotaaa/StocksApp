using Microsoft.EntityFrameworkCore;
using StocksApp.Entities;
using StocksApp.RepositoryContracts;
using StocksApp.ServiceContracts;
using StocksApp.ServiceContracts.DTO;
using StocksApp.ServiceContracts.Extensions;
using StocksApp.Services.Helpers;

namespace StocksApp.Services
{
    public class StocksService : IStocksService
    {
        // Private fields
        private readonly IStocksRepository _stocksRepository;
        private readonly ILogger<StocksService> _logger;

        // Constructor
        public StocksService(IStocksRepository stocksRepository, ILogger<StocksService> logger) 
        {
            _stocksRepository = stocksRepository;
            _logger = logger;
        }

        public async Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest)
        {
            // Log
            _logger.LogInformation("BuyOrder requested to CreateBuyOrder called from StocksService");

            // Check if buyOrderRequest is null
            if (buyOrderRequest == null)
                throw new ArgumentNullException(nameof(buyOrderRequest));

            // Validate all properties of buyOrderRequest
            ValidationHelper.ModelValidation(buyOrderRequest);

            // Convert buyOrderRequest to BuyOrder Entitie and put a new orderID
            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();
            buyOrder.BuyOrderID = Guid.NewGuid();
            await _stocksRepository.CreateBuyOrder(buyOrder);

            // Return a buyOrderReponse
            return buyOrder.ToBuyOrderResponse();
        }

        public async Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest)
        {
            // Log
            _logger.LogInformation("SellOrder requested to CreateSellOrder called from StocksService");

            // Check if sellOrderRequest is null
            if (sellOrderRequest == null)
                throw new ArgumentNullException(nameof(sellOrderRequest));

            // Validate all properties of sellOrderRequest
            ValidationHelper.ModelValidation(sellOrderRequest);

            // Convert sellOrderRequest to SellOrder Entitie and put a new orderID
            SellOrder sellOrder = sellOrderRequest.ToSellOrder();
            sellOrder.SellOrderID = Guid.NewGuid();
            await _stocksRepository.CreateSellOrder(sellOrder);

            // Return a sellOrderResponse
            return sellOrder.ToSellOrderResponse();
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
