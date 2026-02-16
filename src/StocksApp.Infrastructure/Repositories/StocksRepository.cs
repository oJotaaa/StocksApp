using Microsoft.EntityFrameworkCore;
using StocksApp.Entities;
using StocksApp.RepositoryContracts;

namespace StocksApp.Repositories
{
    public class StocksRepository : IStocksRepository
    {
        private readonly ApplicationDbContext _db;

        public StocksRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<BuyOrder> CreateBuyOrder(BuyOrder buyOrder)
        {
            await _db.BuyOrders.AddAsync(buyOrder);
            await _db.SaveChangesAsync();
            return buyOrder;
        }

        public async Task<SellOrder> CreateSellOrder(SellOrder sellOrder)
        {
            await _db.SellOrders.AddAsync(sellOrder);
            await _db.SaveChangesAsync();
            return sellOrder;
        }

        public async Task<List<BuyOrder>> GetBuyOrders()
        {
            List<BuyOrder> buyOrders = await _db.BuyOrders.ToListAsync();
            return buyOrders;
        }

        public async Task<List<SellOrder>> GetSellOrders()
        {
            List<SellOrder> sellOrders = await _db.SellOrders.ToListAsync();
            return sellOrders;
        }
    }
}
