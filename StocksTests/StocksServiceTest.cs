using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using StocksApp.Entities;
using StocksApp.RepositoryContracts;
using StocksApp.ServiceContracts;
using StocksApp.ServiceContracts.DTO;
using StocksApp.ServiceContracts.Extensions;
using StocksApp.Services;
using System.Runtime.ConstrainedExecution;

namespace StocksTests
{
    public class StocksServiceTest
    {
        private readonly IStocksService _stocksService;
        private readonly Mock<IStocksRepository> _stocksRepositoryMock;

        private readonly Fixture _fixture;
        private readonly ILogger<StocksService> _logger;

        private readonly Mock<ILogger<StocksService>> _loggerMock;

        public StocksServiceTest()
        {
            _fixture = new Fixture();
            _loggerMock = new Mock<ILogger<StocksService>>();
            _logger = _loggerMock.Object;

            _stocksRepositoryMock = new Mock<IStocksRepository>();
            _stocksService = new StocksService(_stocksRepositoryMock.Object, _logger);
        }

        #region CreateBuyOrderTests

        // When you supply BuyOrderRequest as null, it should throw ArgumentNullException
        [Fact]
        public async Task CreateBuyOrder_BuyOrderRequestNull()
        {
            // Arrange
            BuyOrderRequest? buyOrderRequest = null;

            // Act
            Func<Task> action = async () =>
            {
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            };

            // Assert
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        // When you supply buyOrderQuantity as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [Fact]
        public async Task CreateBuyOrder_QuantityZero()
        {
            // Arrange
            BuyOrderRequest? buyOrderRequest = _fixture.Build<BuyOrderRequest>()
                .With(temp => temp.Quantity, (uint)0)
                .Create();

            // Act
            Func<Task> action = async () =>
            {
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            };

            // Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        // When you supply buyOrderQuantity as 100001 (as per the specification, maximum is 100000), it should throw ArgumentException.
        [Fact]
        public async Task CreateBuyOrder_MaxOrderQuantity()
        {
            // Arrange
            BuyOrderRequest? buyOrderRequest = _fixture.Build<BuyOrderRequest>()
                .With(temp => temp.Quantity, (uint)100001)
                .Create();

            // Act
            Func<Task> action = async () =>
            {
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            };

            // Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        // When you supply buyOrderPrice as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [Fact]
        public async Task CreateBuyOrder_PriceZero()
        {
            // Arrange
            BuyOrderRequest? buyOrderRequest = _fixture.Build<BuyOrderRequest>()
                .With(temp => temp.Price, 0)
                .Create();

            // Act
            Func<Task> action = async () =>
            {
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            };

            // Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        // When you supply buyOrderPrice as 10001 (as per the specification, maximum is 10000), it should throw ArgumentException.
        [Fact]
        public async Task CreateBuyOrder_MaxOrderPrice()
        {
            // Arrange
            BuyOrderRequest? buyOrderRequest = _fixture.Build<BuyOrderRequest>()
                .With(temp => temp.Price, (uint)10001)
                .Create();

            // Act
            Func<Task> action = async () =>
            {
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            };

            // Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        // When you supply stock symbol=null (as per the specification, stock symbol can't be null), it should throw ArgumentException.
        [Fact]
        public async Task CreateBuyOrder_StockSymbolNull()
        {
            // Arrange
            BuyOrderRequest? buyOrderRequest = _fixture.Build<BuyOrderRequest>()
                .With(temp => temp.StockSymbol, null as string)
                .Create();

            // Act
            Func<Task> action = async () =>
            {
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            };

            // Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        // If you supply all valid values, it should be successful and return an object of BuyOrderResponse type with auto-generated BuyOrderID (guid).
        [Fact]
        public async Task CreateBuyOrder_ValidValues()
        {
            // Arrange
            BuyOrderRequest? buyOrderRequest = _fixture.Create<BuyOrderRequest>();
            BuyOrder? buyOrder = _fixture.Create<BuyOrder>();

            // Mock
            _stocksRepositoryMock.Setup(temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buyOrder);

            // Act
            BuyOrderResponse buyOrderResponse = await _stocksService.CreateBuyOrder(buyOrderRequest);
            

            // Assert
            buyOrderResponse.Should().NotBeNull();
            buyOrderResponse.BuyOrderID.Should().NotBe(Guid.Empty);
        }


        #endregion

        #region CreateSellOrderTests

        // When you supply SellOrderRequest as null, it should throw ArgumentNullException.
        [Fact]
        public async Task CreateSellOrder_SellOrderRequestNull()
        {
            // Arrange
            SellOrderRequest? sellOrderRequest = null;

            // Act
            Func<Task> action = async () =>
            {
                await _stocksService.CreateSellOrder(sellOrderRequest);
            };

            // Assert
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        // When you supply sellOrderQuantity as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [Fact]
        public async Task CreateSellOrder_QuantityZero()
        {
            // Arrange
            SellOrderRequest? sellOrderRequest = _fixture.Build<SellOrderRequest>()
                .With(temp => temp.Quantity, (uint)0)
                .Create();

            // Act
            Func<Task> action = async () =>
            {
                await _stocksService.CreateSellOrder(sellOrderRequest);
            };

            // Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        // When you supply sellOrderQuantity as 100001 (as per the specification, maximum is 100000), it should throw ArgumentException.
        [Fact]
        public async Task CreateSellOrder_MaxOrderQuantity()
        {
            // Arrange
            SellOrderRequest? sellOrderRequest = _fixture.Build<SellOrderRequest>()
                .With(temp => temp.Quantity, (uint)100001)
                .Create();

            // Act
            Func<Task> action = async () =>
            {
                await _stocksService.CreateSellOrder(sellOrderRequest);
            };

            // Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        // When you supply sellOrderPrice as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [Fact]
        public async Task CreateSellOrder_SellOrderPriceZero()
        {
            // Arrange
            SellOrderRequest? sellOrderRequest = _fixture.Build<SellOrderRequest>()
                .With(temp => temp.Price, 0)
                .Create();

            // Act
            Func<Task> action = async () =>
            {
                await _stocksService.CreateSellOrder(sellOrderRequest);
            };

            // Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        // When you supply sellOrderPrice as 10001 (as per the specification, maximum is 10000), it should throw ArgumentException.
        [Fact]
        public async Task CreateSellOrder_MaxOrderPrice()
        {
            // Arrange
            SellOrderRequest? sellOrderRequest = _fixture.Build<SellOrderRequest>()
                .With(temp => temp.Price, 10001)
                .Create();

            // Act
            Func<Task> action = async () =>
            {
                await _stocksService.CreateSellOrder(sellOrderRequest);
            };

            // Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        // When you supply stock symbol=null (as per the specification, stock symbol can't be null), it should throw ArgumentException.
        [Fact]
        public async Task CreateSellOrder_StockSymbolNull()
        {
            // Arrange
            SellOrderRequest? sellOrderRequest = _fixture.Build<SellOrderRequest>()
                .With(temp => temp.StockSymbol, null as string)
                .Create();

            // Act
            Func<Task> action = async () =>
            {
                await _stocksService.CreateSellOrder(sellOrderRequest);
            };

            // Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        // If you supply all valid values, it should be successful and return an object of SellOrderResponse type with auto-generated SellOrderID (guid).
        [Fact]
        public async Task CreateSellOrder_ValidValues()
        {
            // Arrange
            SellOrderRequest? sellOrderRequest = _fixture.Create<SellOrderRequest>();

            SellOrder sellOrder = _fixture.Create<SellOrder>();

            // Mock
            _stocksRepositoryMock.Setup(temp => temp.CreateSellOrder(It.IsAny<SellOrder>())).ReturnsAsync(sellOrder);

            // Act
            SellOrderResponse? sellOrderResponse = await _stocksService.CreateSellOrder(sellOrderRequest);

            sellOrderResponse.Should().NotBeNull();
            sellOrderResponse.SellOrderID.Should().NotBe(Guid.Empty);
        }
        #endregion

        #region GetAllBuyOrdersTests

        // When you invoke this method, by default, the returned list should be empty.
        [Fact]
        public async Task GetAllBuyOrders_EmptyList()
        {
            // Arrange
            List<BuyOrder> buyOrdersEmpty = new List<BuyOrder>();

            // Mock
            _stocksRepositoryMock.Setup(temp => temp.GetBuyOrders()).ReturnsAsync(buyOrdersEmpty);

            // Act
            List<BuyOrderResponse> buyOrderResponses = await _stocksService.GetBuyOrders();

            // Assert
            buyOrderResponses.Should().BeEmpty();
        }

        // When you first add few buy orders using CreateBuyOrder() method; and then invoke GetAllBuyOrders() method; the returned list should contain all the same buy orders.
        [Fact]
        public async Task GetBuyOrders_AddFewOrders()
        {
            // Arrange
            List<BuyOrder> buyOrders = new List<BuyOrder>() 
            {
                _fixture.Create<BuyOrder>(),
                _fixture.Create<BuyOrder>(),
                _fixture.Create<BuyOrder>()
            };

            List<BuyOrderResponse> buyOrderResponses = buyOrders.Select(temp => temp.ToBuyOrderResponse()).ToList();

            // Mock
            _stocksRepositoryMock.Setup(temp => temp.GetBuyOrders()).ReturnsAsync(buyOrders);

            // Act
            List<BuyOrderResponse> allOrdersFromGet = await _stocksService.GetBuyOrders();

            // Assert
            allOrdersFromGet.Should().BeEquivalentTo(buyOrderResponses);
        }

        #endregion

        #region GetAllSellOrdersTests

        // When you invoke this method, by default, the returned list should be empty.
        [Fact]
        public async Task GetAllSellOrders_EmptyList()
        {
            // Arrange
            List<SellOrder> sellOrdersEmpty = new List<SellOrder>();

            // Mock
            _stocksRepositoryMock.Setup(temp => temp.GetSellOrders()).ReturnsAsync(sellOrdersEmpty);

            // Act
            List<SellOrderResponse> sellOrderResponses = await _stocksService.GetSellOrders();

            // Assert
            sellOrderResponses.Should().BeEmpty();
        }

        // When you first add few buy orders using CreateSellOrder() method; and then invoke GetAllSellOrders() method; the returned list should contain all the same sell orders.
        [Fact]
        public async Task GetSellOrders_AddFewOrders()
        {
            // Arrange
            List<SellOrder> sellOrders = new List<SellOrder>()
            {
                _fixture.Create<SellOrder>(),
                _fixture.Create<SellOrder>(),
                _fixture.Create<SellOrder>()
            };

            List<SellOrderResponse> sellOrderResponses = sellOrders.Select(temp => temp.ToSellOrderResponse()).ToList();

            // Mock
            _stocksRepositoryMock.Setup(temp => temp.GetSellOrders()).ReturnsAsync(sellOrders);

            // Act
            List<SellOrderResponse> allOrdersFromGet = await _stocksService.GetSellOrders();

            // Assert
            allOrdersFromGet.Should().BeEquivalentTo(sellOrderResponses);
        }
        #endregion
    }
}
