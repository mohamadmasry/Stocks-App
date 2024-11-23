using Fizzler.Systems.HtmlAgilityPack;
using FluentAssertions;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc.Testing;
using StocksApp.Controllers;

namespace StocksAppTest;

public class TradeControllerIntegrationTest : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public TradeControllerIntegrationTest(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    #region Index

    [Fact]
    public async Task Index_WithSSymbol_ShouldReturnViewWithCorrectElements()
    {
        //Arrange
        //Act
        HttpResponseMessage response = await _client.GetAsync("Trade/Index/AAPL");
       
        //Assert
        
        //Assert Response successful and is a html response
        response.Should().BeSuccessful();
        response.Content.Headers.ContentType?.MediaType.Should().Be("text/html");

        string responseBody = await response.Content.ReadAsStringAsync();
        HtmlDocument html = new HtmlDocument();
        html.LoadHtml(responseBody);
        var document = html.DocumentNode;
        
        //Assert specific elements
        document.QuerySelectorAll(".price").Should().NotBeEmpty();
        document.QuerySelector(".stock-title").InnerText.Should().Contain("AAPL");
    }

    [Fact]
    public async Task Index_WithoutSSymbol_ShouldReturnViewWithCorrectElements()
    {
        //Act
        HttpResponseMessage response = await _client.GetAsync("Trade/Index");
        
        //Assert:
        
        //Assert response successful and is a html response
        response.Should().BeSuccessful();
        response.Content.Headers.ContentType?.MediaType.Should().Be("text/html");
        
        string responseBody = await response.Content.ReadAsStringAsync();
        HtmlDocument html = new HtmlDocument();
        html.LoadHtml(responseBody);
        var document = html.DocumentNode;
        //Assert specific elements
        document.QuerySelectorAll(".price").Should().NotBeEmpty();
        document.QuerySelector(".stock-title").InnerText.Should().Contain("MSFT");
    }
    #endregion

    #region Orders

    [Fact]
    public async Task Orders_ShouldReturnViewWithCorrectElements()
    {
        //Arrange
        //Act
        HttpResponseMessage response = await _client.GetAsync("Trade/Orders");
        //Assert
        //Response successful and is a View(html response)
        response.Should().BeSuccessful();
        response.Content.Headers.ContentType?.MediaType.Should().Be("text/html");

        string responseBody = await response.Content.ReadAsStringAsync();
        HtmlDocument html = new HtmlDocument();
        html.LoadHtml(responseBody);
        var document = html.DocumentNode;
        
        //Assert specific elements
        document.QuerySelectorAll(".orders-list").Should().NotBeEmpty();
        document.QuerySelectorAll("#buy-orders-list").Should().NotBeEmpty();
        document.QuerySelectorAll("#sell-orders-list").Should().NotBeEmpty();
    }

    #endregion
}