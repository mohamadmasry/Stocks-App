using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Repository;
using RepositoryContracts;
using Serilog;
using Service;
using ServiceContract;
using StocksApp;
using StocksApp.Controllers;
using StocksApp.Models;

namespace StocksAppTest;

public class StocksControllerTest
{
    private readonly IFinnhubService _finnhubService;
    private readonly Mock<IFinnhubService> _finnhubServiceMock;
    private readonly Mock<IOptions<TradingOptions>> _options;
    private readonly Mock<IDiagnosticContext> _diagnosticMock;
    public StocksControllerTest()
    {
        _diagnosticMock = new Mock<IDiagnosticContext>();
        _options = new Mock<IOptions<TradingOptions>>();
        var tradingOptions = new TradingOptions
        {
            DefaultOrderQuantity ="123",
            Top25PopularStocks = "AAPL,MSFT,AMZN,TSLA,GOOGL,GOOG,NVDA,BRK.B,META,UNH,JNJ,JPM,V,PG,XOM,HD,CVX,MA,BAC,ABBV,PFE,AVGO,KO,PEP,WMT"
        };
        _options.Setup(temp=> 
            temp.Value).Returns(tradingOptions);
        _finnhubServiceMock = new Mock<IFinnhubService>();
        _finnhubService = _finnhubServiceMock.Object;
    }
    [Fact]
    public async Task Explore_NullSymbol_ToReturnViewWithoutInfo()
    {
        //Arrange
        string symbol = null;
        _finnhubServiceMock.Setup(temp =>
            temp.GetStocks()).ReturnsAsync(new List<Dictionary<string, string>>());
        StocksController controller = new StocksController(_finnhubService, _options.Object,_diagnosticMock.Object);
        //Act
        IActionResult result = await controller.Explore(symbol);
        //Assert
        ViewResult viewResult = Assert.IsType<ViewResult>(result);
        viewResult.ViewName.Should().Be("Explore");
        viewResult.ViewData.Model.Should().BeOfType<List<Stock>>();
    }

    [Fact]
    public async Task Explore_WithSymbol_ToReturnViewWithInfo()
    {
        //Arrange
        string symbol = "AAPL";
        _finnhubServiceMock.Setup(temp =>
            temp.GetStocks()).ReturnsAsync(new List<Dictionary<string, string>>());
        _finnhubServiceMock.Setup(temp =>
            temp.GetCompanyProfile(It.IsAny<string>())).ReturnsAsync(new Dictionary<string, object>(){{"logo", "www.google.con"}} );
        _finnhubServiceMock.Setup(temp=>
            temp.GetStockPriceQuote(It.IsAny<string>())).ReturnsAsync(new Dictionary<string, object>());
        StocksController controller = new StocksController(_finnhubService, _options.Object,_diagnosticMock.Object);
        //Act
        IActionResult result = await controller.Explore(symbol);
        //Assert
        ViewResult viewResult = Assert.IsType<ViewResult>(result);
        viewResult.ViewName.Should().Be("Explore");
        viewResult.ViewData.Model.Should().BeOfType<List<Stock>>();
        viewResult.ViewData["SelectedSymbol"].Should().Be(symbol);
    }
}