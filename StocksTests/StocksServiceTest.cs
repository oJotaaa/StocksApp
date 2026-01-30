using StocksApp.ServiceContracts;
using StocksApp.ServiceContracts.DTO;
using StocksApp.Services;

namespace StocksTests
{
    public class StocksServiceTest
    {
        private readonly IStocksService _stocksService;

        public StocksServiceTest()
        {
            _stocksService = new StocksService();
        }

        #region CreateBuyOrderTests

        // When you supply BuyOrderRequest as null, it should throw ArgumentNullException
        [Fact]
        public async Task CreateBuyOrder_BuyOrderRequestNull()
        {
            // Arrange
            BuyOrderRequest? buyOrderRequest = null;

            // Assert/Act
            await Assert.ThrowsAsync<ArgumentNullException>(() => _stocksService.CreateBuyOrder(buyOrderRequest));
        }

        // When you supply buyOrderQuantity as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [Fact]
        public async Task CreateBuyOrder_QuantityZero()
        {
            // Arrange
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest() 
            { 
                StockName = "SomeStock",
                StockSymbol = "SSS",
                Quantity = 0,
                Price = 200
            };

            // Assert/Act
            await Assert.ThrowsAsync<ArgumentException>(() => _stocksService.CreateBuyOrder(buyOrderRequest));
        }

        // When you supply buyOrderQuantity as 100001 (as per the specification, maximum is 100000), it should throw ArgumentException.
        [Fact]
        public async Task CreateBuyOrder_MaxOrderQuantity()
        {
            // Arrange
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest()
            {
                StockName = "SomeStock",
                StockSymbol = "SSS",
                Quantity = 100001,
                Price = 200
            };

            // Assert/Act
            await Assert.ThrowsAsync<ArgumentException>(() => _stocksService.CreateBuyOrder(buyOrderRequest));
        }

        // When you supply buyOrderPrice as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [Fact]
        public async Task CreateBuyOrder_PriceZero()
        {
            // Arrange
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest()
            {
                StockName = "SomeStock",
                StockSymbol = "SSS",
                Quantity = 10,
                Price = 0
            };

            // Assert/Act
            await Assert.ThrowsAsync<ArgumentException>(() => _stocksService.CreateBuyOrder(buyOrderRequest));
        }

        // When you supply buyOrderPrice as 10001 (as per the specification, maximum is 10000), it should throw ArgumentException.
        [Fact]
        public async Task CreateBuyOrder_MaxOrderPrice()
        {
            // Arrange
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest()
            {
                StockName = "SomeStock",
                StockSymbol = "SSS",
                Quantity = 100,
                Price = 10001
            };

            // Assert/Act
            await Assert.ThrowsAsync<ArgumentException>(() => _stocksService.CreateBuyOrder(buyOrderRequest));
        }

        // When you supply stock symbol=null (as per the specification, stock symbol can't be null), it should throw ArgumentException.
        [Fact]
        public async Task CreateBuyOrder_StockSymbolNull()
        {
            // Arrange
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest()
            {
                StockName = "SomeStock",
                StockSymbol = null,
                Quantity = 100,
                Price = 50
            };

            // Assert/Act
            await Assert.ThrowsAsync<ArgumentException>(() => _stocksService.CreateBuyOrder(buyOrderRequest));
        }

        // When you supply dateAndTimeOfOrder as "1999-12-31" (YYYY-MM-DD) - (as per the specification, it should be equal or newer date than 2000-01-01), it should throw ArgumentException.
        [Fact]
        public async Task CreateBuyOrder_DateTimeOlder()
        {
            // Arrange
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest()
            {
                StockName = "SomeStock",
                StockSymbol = "SSS",
                DateAndTimeOfOrder = DateTime.Parse("1999-12-31"),
                Quantity = 100,
                Price = 50
            };

            // Assert/Act
            await Assert.ThrowsAsync<ArgumentException>(() => _stocksService.CreateBuyOrder(buyOrderRequest));
        }

        // If you supply all valid values, it should be successful and return an object of BuyOrderResponse type with auto-generated BuyOrderID (guid).
        [Fact]
        public async Task CreateBuyOrder_ValidValues()
        {
            // Arrange
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest()
            {
                StockName = "SomeStock",
                StockSymbol = "SSS",
                DateAndTimeOfOrder = DateTime.Parse("2005-12-31"),
                Quantity = 100,
                Price = 50
            };

            // Act
            BuyOrderResponse buyOrderResponse = await _stocksService.CreateBuyOrder(buyOrderRequest);

            // Assert
            Assert.NotNull(buyOrderResponse);
            Assert.NotEqual(buyOrderResponse.BuyOrderID, Guid.Empty);
        }


        #endregion
    }
}
