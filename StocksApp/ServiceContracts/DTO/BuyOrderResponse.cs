namespace StocksApp.ServiceContracts.DTO
{
    public class BuyOrderResponse
    {
        public Guid BuyOrderID { get; set; }
        public string? StockSymbol { get; set; }
        public string? StockName { get; set; }
        public DateTime DateAndTimeOfOrder { get; set; }
        public uint Quantity { get; set; }
        public double Price { get; set; }
        public double TradeAmount { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;

            if (obj.GetType() != typeof(BuyOrderResponse)) return false;

            BuyOrderResponse buyOrderObject = (BuyOrderResponse)obj;
            return BuyOrderID == buyOrderObject.BuyOrderID &&
                   StockSymbol == buyOrderObject.StockSymbol &&
                   StockName == buyOrderObject.StockName &&
                   DateAndTimeOfOrder == buyOrderObject.DateAndTimeOfOrder &&
                   Quantity == buyOrderObject.Quantity &&
                   Price == buyOrderObject.Price &&
                   TradeAmount == buyOrderObject.TradeAmount;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
