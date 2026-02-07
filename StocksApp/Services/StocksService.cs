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

        // Constructor
        public StocksService(IStocksRepository stocksRepository) 
        {
            _stocksRepository = stocksRepository;
        }

        public async Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest)
        {
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
            List<BuyOrder> buyOrders = await _stocksRepository.GetBuyOrders();
            return buyOrders.Select(temp => temp.ToBuyOrderResponse()).ToList();
        }

        public async Task<List<SellOrderResponse>> GetSellOrders()
        {
            List<SellOrder> sellOrders = await _stocksRepository.GetSellOrders();

            return sellOrders.Select(temp => temp.ToSellOrderResponse()).ToList();
        }
    }
}
