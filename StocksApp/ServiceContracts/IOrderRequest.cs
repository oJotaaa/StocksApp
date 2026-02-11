namespace StocksApp.ServiceContracts
{
    public interface IOrderRequest
    {
        string? StockSymbol { get; set; }
        string? StockName { get; set; }
        double Price { get; set; }
        uint Quantity { get; set; }
    }
}
