using Microsoft.EntityFrameworkCore;
using StocksApp.Entities;
using StocksApp.ServiceContracts;
using StocksApp.ServiceContracts.DTO;
using StocksApp.ServiceContracts.Extensions;
using StocksApp.Services.Helpers;

namespace StocksApp.Services
{
    public class StocksService : IStocksService
    {
        // Private fields
        private readonly StockMarketDbContext _db;

        // Constructor
        public StocksService(StockMarketDbContext stockMarketDbContext) 
        {
            _db = stockMarketDbContext;
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
            await _db.BuyOrders.AddAsync(buyOrder);
            await _db.SaveChangesAsync();

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
            await _db.SellOrders.AddAsync(sellOrder);
            await _db.SaveChangesAsync();

            // Return a sellOrderResponse
            return sellOrder.ToSellOrderResponse();
        }

        public async Task<List<BuyOrderResponse>> GetBuyOrders()
        {
            List<BuyOrder> buyOrders = await _db.BuyOrders.Select(temp => temp).ToListAsync();
            return buyOrders.Select(temp => temp.ToBuyOrderResponse()).ToList();
        }

        public async Task<List<SellOrderResponse>> GetSellOrders()
        {
            List<SellOrder> sellOrders = await _db.SellOrders.Select(temp => temp).ToListAsync();

            return sellOrders.Select(temp => temp.ToSellOrderResponse()).ToList();
        }
    }
}
