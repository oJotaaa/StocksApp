using StocksApp.ServiceContracts;
using StocksApp.ServiceContracts.DTO;
using StocksApp.Services;
using System.Runtime.ConstrainedExecution;

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

        #region CreateSellOrderTests

        // When you supply SellOrderRequest as null, it should throw ArgumentNullException.
        [Fact]
        public async Task CreateSellOrder_SellOrderRequestNull()
        {
            // Arrange
            SellOrderRequest? sellOrderRequest = null;

            // Assert/Act
            await Assert.ThrowsAsync<ArgumentNullException>(() => _stocksService.CreateSellOrder(sellOrderRequest));
        }

        // When you supply sellOrderQuantity as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [Fact]
        public async Task CreateSellOrder_QuantityZero()
        {
            // Arrange
            SellOrderRequest? sellOrderRequest = new SellOrderRequest()
            {
                StockName = "SomeStock",
                StockSymbol = "SSS",
                Quantity = 0,
                Price = 200
            };

            // Assert/Act
            await Assert.ThrowsAsync<ArgumentException>(() => _stocksService.CreateSellOrder(sellOrderRequest));
        }

        // When you supply sellOrderQuantity as 100001 (as per the specification, maximum is 100000), it should throw ArgumentException.
        [Fact]
        public async Task CreateSellOrder_MaxOrderQuantity()
        {
            // Arrange
            SellOrderRequest? sellOrderRequest = new SellOrderRequest()
            {
                StockName = "SomeStock",
                StockSymbol = "SSS",
                Quantity = 100001,
                Price = 200
            };

            // Assert/Act
            await Assert.ThrowsAsync<ArgumentException>(() => _stocksService.CreateSellOrder(sellOrderRequest));
        }

        // When you supply sellOrderPrice as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [Fact]
        public async Task CreateSellOrder_SellOrderPriceZero()
        {
            // Arrange
            SellOrderRequest? sellOrderRequest = new SellOrderRequest()
            {
                StockName = "SomeStock",
                StockSymbol = "SSS",
                Quantity = 100,
                Price = 0
            };

            // Assert/Act
            await Assert.ThrowsAsync<ArgumentException>(() => _stocksService.CreateSellOrder(sellOrderRequest));
        }

        // When you supply sellOrderPrice as 10001 (as per the specification, maximum is 10000), it should throw ArgumentException.
        [Fact]
        public async Task CreateSellOrder_MaxOrderPrice()
        {
            // Arrange
            SellOrderRequest? sellOrderRequest = new SellOrderRequest()
            {
                StockName = "SomeStock",
                StockSymbol = "SSS",
                Quantity = 100,
                Price = 10001
            };

            // Assert/Act
            await Assert.ThrowsAsync<ArgumentException>(() => _stocksService.CreateSellOrder(sellOrderRequest));
        }

        // When you supply stock symbol=null (as per the specification, stock symbol can't be null), it should throw ArgumentException.
        [Fact]
        public async Task CreateSellOrder_StockSymbolNull()
        {
            // Arrange
            SellOrderRequest? sellOrderRequest = new SellOrderRequest()
            {
                StockName = "SomeStock",
                StockSymbol = null,
                Quantity = 100,
                Price = 50
            };

            // Assert/Act
            await Assert.ThrowsAsync<ArgumentException>(() => _stocksService.CreateSellOrder(sellOrderRequest));
        }

        // When you supply dateAndTimeOfOrder as "1999-12-31" (YYYY-MM-DD) - (as per the specification, it should be equal or newer date than 2000-01-01), it should throw ArgumentException.
        [Fact]
        public async Task CreateSellOrder_MinimumDate()
        {
            // Arrange
            SellOrderRequest? sellOrderRequest = new SellOrderRequest()
            {
                StockName = "SomeStock",
                StockSymbol = null,
                DateAndTimeOfOrder = DateTime.Parse("1999-12-31"),
                Quantity = 100,
                Price = 50
            };

            // Assert/Act
            await Assert.ThrowsAsync<ArgumentException>(() => _stocksService.CreateSellOrder(sellOrderRequest));
        }

        // If you supply all valid values, it should be successful and return an object of SellOrderResponse type with auto-generated SellOrderID (guid).
        [Fact]
        public async Task CreateSellOrder_ValidValues()
        {
            // Arrange
            SellOrderRequest? sellOrderRequest = new SellOrderRequest()
            {
                StockName = "SomeStock",
                StockSymbol = "SSS",
                DateAndTimeOfOrder = DateTime.Parse("2005-12-31"),
                Quantity = 100,
                Price = 50
            };

            // Act
            SellOrderResponse? sellOrderResponse = await _stocksService.CreateSellOrder(sellOrderRequest);

            Assert.NotNull(sellOrderResponse);
            Assert.NotEqual(sellOrderResponse.SellOrderID, Guid.Empty);
        }
        #endregion

        #region GetAllBuyOrdersTests

        // When you invoke this method, by default, the returned list should be empty.
        [Fact]
        public async Task GetAllBuyOrders_EmptyList()
        {
            // Act
            List<BuyOrderResponse> buyOrderResponses = await _stocksService.GetBuyOrders();

            // Assert
            Assert.Empty(buyOrderResponses);
        }

        // When you first add few buy orders using CreateBuyOrder() method; and then invoke GetAllBuyOrders() method; the returned list should contain all the same buy orders.
        [Fact]
        public async Task GetBuyOrders_AddFewOrders()
        {
            // Arrange
            List<BuyOrderResponse> buyOrderResponses = new List<BuyOrderResponse>();

            BuyOrderRequest buyOrderRequest1 = new BuyOrderRequest()
            {
                StockName = "Stockname",
                StockSymbol = "SSS",
                DateAndTimeOfOrder = DateTime.Parse("2005-01-01"),
                Quantity = 10,
                Price = 50
            };

            BuyOrderRequest buyOrderRequest2 = new BuyOrderRequest()
            {
                StockName = "StockSample",
                StockSymbol = "DDD",
                DateAndTimeOfOrder = DateTime.Parse("2007-01-01"),
                Quantity = 5,
                Price = 60
            };

            BuyOrderResponse buyOrderResponse1 = await _stocksService.CreateBuyOrder(buyOrderRequest1);
            BuyOrderResponse buyOrderResponse2 = await _stocksService.CreateBuyOrder(buyOrderRequest2);

            buyOrderResponses.Add(buyOrderResponse1);
            buyOrderResponses.Add(buyOrderResponse2);

            // Act
            List<BuyOrderResponse> allOrders = await _stocksService.GetBuyOrders();

            // Assert
            for (int i = 0;  i < allOrders.Count; i++)
            {
                Assert.Equal(buyOrderResponses[i], allOrders[i]);
            }
        }

        #endregion

        #region GetAllSellOrdersTests

        // When you invoke this method, by default, the returned list should be empty.
        [Fact]
        public async Task GetAllSellOrders_EmptyList()
        {
            // Act
            List<SellOrderResponse> sellOrderResponses = await _stocksService.GetSellOrders();

            // Assert
            Assert.Empty(sellOrderResponses);
        }

        // When you first add few buy orders using CreateSellOrder() method; and then invoke GetAllSellOrders() method; the returned list should contain all the same sell orders.
        [Fact]
        public async Task GetSellOrders_AddFewOrders()
        {
            // Arrange
            List<SellOrderResponse> sellOrderResponses = new List<SellOrderResponse>();

            SellOrderRequest sellOrderRequest1 = new SellOrderRequest()
            {
                StockName = "Stockname",
                StockSymbol = "SSS",
                DateAndTimeOfOrder = DateTime.Parse("2005-01-01"),
                Quantity = 10,
                Price = 50
            };

            SellOrderRequest sellOrderRequest2 = new SellOrderRequest()
            {
                StockName = "StockSample",
                StockSymbol = "DDD",
                DateAndTimeOfOrder = DateTime.Parse("2007-01-01"),
                Quantity = 5,
                Price = 60
            };

            SellOrderResponse sellOrderResponse1 = await _stocksService.CreateSellOrder(sellOrderRequest1);
            SellOrderResponse sellOrderResponse2 = await _stocksService.CreateSellOrder(sellOrderRequest2);

            sellOrderResponses.Add(sellOrderResponse1);
            sellOrderResponses.Add(sellOrderResponse2);

            // Act
            List<SellOrderResponse> allOrders = await _stocksService.GetSellOrders();

            // Assert
            for (int i = 0; i < allOrders.Count; i++)
            {
                Assert.Equal(sellOrderResponses[i], allOrders[i]);
            }
        }
        #endregion
    }
}
