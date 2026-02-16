using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Text;

namespace StocksTests
{
    public class StocksControllerIntegrationTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public StocksControllerIntegrationTest(WebApplicationFactory<Program> webApplicationFactory)
        {
            _client = webApplicationFactory.CreateClient();
        }

        [Fact]
        public async Task Index_ToReturnView()
        {
            // Act
            HttpResponseMessage response = await _client.GetAsync("Trade/Index/MSFT");

            // Assert
            response.IsSuccessStatusCode.Should().BeTrue();

            string responseBody = await response.Content.ReadAsStringAsync();
            responseBody.Should().Contain("id=\"priceDisplay\"");
        }
    }
}
