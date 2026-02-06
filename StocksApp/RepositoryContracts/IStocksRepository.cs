using StocksApp.Entities;

namespace StocksApp.RepositoryContracts
{
    /// <summary>
    /// Represents data access logic for managing Stocks Entity
    /// </summary>
    public interface IStocksRepository
    {
        /// <summary>
        /// Add a new BuyOrder to the data store
        /// </summary>
        /// <param name="buyOrder">The buyOrder to add.</param>
        /// <returns>Returns the buyOrder</returns>
        Task<BuyOrder> CreateBuyOrder(BuyOrder buyOrder);

        /// <summary>
        /// Add a new SellOrder to the data store
        /// </summary>
        /// <param name="sellOrder">The sellOrder to add.</param>
        /// <returns>Returns the sellOrder</returns>
        Task<SellOrder> CreateSellOrder(SellOrder sellOrder);

        /// <summary>
        /// Retrieves all BuyOrders.
        /// </summary>
        /// <returns>Returns a List of BuyOrder with all orders.</returns>
        Task<List<BuyOrder>> GetBuyOrders();

        /// <summary>
        /// Retrieves all SellOrders.
        /// </summary>
        /// <returns>Returns a List of SellOrder with all orders</returns>
        Task<List<SellOrder>> GetSellOrders();

    }
}
