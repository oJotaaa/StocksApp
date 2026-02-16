using AutoFixture;
using Castle.Core.Logging;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using StocksApp;
using StocksApp.Controllers;
using StocksApp.RepositoryContracts;
using StocksApp.ServiceContracts;
using StocksApp.Services;
using StocksApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace StocksTests
{
    public class StocksControllerTest
    {
        private readonly IFixture _fixture;
        private readonly IOptions<TradingOptions> _options;
        private readonly ILogger<StocksController> _logger;

        private readonly Mock<IStocksCreateService> _stocksServiceMock;
        private readonly Mock<IFinnhubGetterService> _finnhubServiceMock;
        private readonly Mock<ILogger<StocksController>> _loggerMock;

        private readonly IStocksCreateService _stocksService;
        private readonly IFinnhubGetterService _finnhubService;

        private readonly StocksController _stocksController;

        public StocksControllerTest()
        {
            _fixture = new Fixture();
            _options = Options.Create(new TradingOptions());

            _loggerMock = new Mock<ILogger<StocksController>>();
            _logger = _loggerMock.Object;

            _stocksServiceMock = new Mock<IStocksCreateService>();
            _finnhubServiceMock = new Mock<IFinnhubGetterService>();

            _stocksService = _stocksServiceMock.Object;
            _finnhubService = _finnhubServiceMock.Object;

            _stocksController = new StocksController(_options, _finnhubService, _logger);
        }

        [Fact]
        public async Task Explore_StockNull_ShouldReturnExploreViewWithStocksList()
        {
            // Arrange
            _options.Value.Top25PopularStocks = "MSFT";

            var stocks = new List<Dictionary<string, string>>()
            {
                _fixture.Build<Dictionary<string, string>>()
                    .Do(temp => temp.Add("symbol", "MSFT"))
                    .Do(temp => temp.Add("description", "Microsoft"))
                    .Create()
            };
                
            // Mock
            _finnhubServiceMock.Setup(temp => temp.GetStocks()).ReturnsAsync(stocks);

            // Act
            IActionResult actionResult = await _stocksController.Explore(null);

            // Assert
            ViewResult viewResult = Assert.IsType<ViewResult>(actionResult);

            viewResult.ViewName.Should().Be("Explore");
            viewResult.ViewData.Model.Should().BeAssignableTo<List<Stock>>();
        }
    }
}
