using StocksApp.Entities;

namespace StocksApp.ServiceContracts.DTO
{
    public class SellOrderResponse
    {
        public Guid SellOrderID { get; set; }
        public string? StockSymbol { get; set; }
        public string? StockName { get; set; }
        public DateTime DateAndTimeOfOrder { get; set; }
        public uint Quantity { get; set; }
        public double Price { get; set; }
        public double TradeAmount { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;

            if (obj.GetType() != typeof(SellOrderResponse)) return false;

            SellOrderResponse sellOrderObject = (SellOrderResponse)obj;
            return SellOrderID == sellOrderObject.SellOrderID &&
                   StockSymbol == sellOrderObject.StockSymbol &&
                   StockName == sellOrderObject.StockName &&
                   DateAndTimeOfOrder == sellOrderObject.DateAndTimeOfOrder &&
                   Quantity == sellOrderObject.Quantity &&
                   Price == sellOrderObject.Price &&
                   TradeAmount == sellOrderObject.TradeAmount;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
