using AutoFixture;
using AutoFixture.Xunit2;
using Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Routing.Template;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using RepositoryContracts;
using Service;
using ServiceContract;
using ServiceContract.DTO;

namespace StocksAppTest;

public class IStocksServiceTest
{
    private readonly IStocksService _service;
    private readonly IStocksRepository _repository;
    private readonly Mock<IStocksRepository> _repositoryMock;
    private readonly IFixture _fixture;
    public IStocksServiceTest()
    {
        _repositoryMock = new Mock<IStocksRepository>();
        var loggerMock = new Mock<ILogger<StocksService>>();
        _repository = _repositoryMock.Object;
        _service = new StocksService(_repository,loggerMock.Object);
        _fixture = new Fixture();
    }

    #region CreateBuyOrder
    
    [Fact]
    // When you supply BuyOrderRequest as null, it should throw ArgumentNullException.
    public async Task CreateBuyOrder_NullBuyOrder_ToThrowArgumentNullException()
    {
        //Arrange
        BuyOrderRequest? buyOrderRequest = null;
        //Act
        Func<Task> action = async () =>
        {
            
            await _service.CreateBuyOrder(buyOrderRequest);
        };
        //Assert
        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    // When you supply buyOrderQuantity as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
    public async Task CreateBuyOrder_belowMinimumbuyOrderQunatity_ToThrowArgumentException()
    {
        BuyOrderRequest buyOrderRequest = _fixture.Build<BuyOrderRequest>()
            .With(temp=>temp.DateAndTimeOfOrder,DateTime.Today)
            .With(temp=>temp.Quantity,0).Create();
        //Act
        Func<Task> action = async () =>
        {
            await _service.CreateBuyOrder(buyOrderRequest);
        };
        //Assert
        await action.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    // When you supply buyOrderQuantity as 100001 (as per the specification, maximum is 100000), it should throw ArgumentException.
    public async Task CreateBuyOrder_aboveMaxbuyOrderQuantity_ToThrowArgumentException()
    {
        //Arrange
        BuyOrderRequest buyOrderRequest = _fixture.Build<BuyOrderRequest>()
            .With(temp=> temp.DateAndTimeOfOrder, DateTime.Today)
            .With(temp=>temp.Quantity,1000000).Create();
        //Act
        Func<Task> action = async () =>
        {
            await _service.CreateBuyOrder(buyOrderRequest);
        };
        //Assert
        await action.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    // When you supply buyOrderPrice as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
    public async Task CreateBuyOrder_belowMinPricebuyOrder_ToThrowArgumentException()
    {
        //Arrange
        BuyOrderRequest buyOrderRequest = _fixture.Build<BuyOrderRequest>()
            .With(temp=> temp.DateAndTimeOfOrder, DateTime.Today)
            .With(temp=>temp.Price,0).Create();
        //Act
        Func<Task> action = async () =>
        {
            await _service.CreateBuyOrder(buyOrderRequest);
        };
        //Assert
        await action.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    // When you supply buyOrderPrice as 10001 (as per the specification, maximum is 10000), it should throw ArgumentException.
    public async Task CreateBuyOrder_aboveMaxPricebuyOrder_ToThrowArgumentException()
    {
        //Arrange
        BuyOrderRequest buyOrderRequest = _fixture.Build<BuyOrderRequest>()
            .With(temp=> temp.DateAndTimeOfOrder, DateTime.Today)
            .With(temp=>temp.Price,10001).Create();
        //Act
        Func<Task> action = async () =>
        {
            await _service.CreateBuyOrder(buyOrderRequest);
        };
        //Assert
        await action.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    // When you supply stock symbol=null (as per the specification, stock symbol can't be null), it should throw ArgumentException.
    public async Task CreateBuyOrder_nullorEmptyStockSymbol_ToThrowArgumentNullException()
    {
        //Arrange
        BuyOrderRequest buyOrderRequest = _fixture.Build<BuyOrderRequest>()
            .With(temp=> temp.DateAndTimeOfOrder, DateTime.Today)
            .With(temp=>temp.StockSymbol,null as string).Create();
        //Act
        Func<Task> action = async () =>
        {
            await _service.CreateBuyOrder(buyOrderRequest);
        };
        //Assert
        await action.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    // When you supply dateAndTimeOfOrder as "1999-12-31" (YYYY-MM-DD) - (as per the specification, it should be equal or newer date than 2000-01-01), it should throw ArgumentException.
    public async Task CreateBuyOrder_belowMinDateofOrder_ToThrowArgumentException()
    {
        //Arrange
        BuyOrderRequest buyOrderRequest = _fixture.Build<BuyOrderRequest>()
            .With(temp=> temp.DateAndTimeOfOrder, DateTime.Parse("1999-12-30")).Create();
        //Act
        Func<Task> action = async () =>
        {
            await _service.CreateBuyOrder(buyOrderRequest);
        };
        //Assert
        await action.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    //If you supply all valid values, it should be successful and return an object of BuyOrderResponse type with auto-generated BuyOrderID (guid).
    public async void CreateBuyOrder_ValidData_ToBeSuccessful()
    {
        //Arrange
        BuyOrderRequest buyOrderRequest = _fixture.Build<BuyOrderRequest>()
            .With(temp=>temp.DateAndTimeOfOrder, DateTime.Today).Create();
        BuyOrder buyOrder = buyOrderRequest.toBuyOrder();
        BuyOrderResponse expectedBuyOrderResponse = buyOrder.toBuyOrderResponse();
        _repositoryMock.Setup(temp=>
            temp.CreateBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buyOrder);
        //Act
        BuyOrderResponse? actualBuyOrderResponse =  await _service.CreateBuyOrder(buyOrderRequest);
        expectedBuyOrderResponse.BuyOrderID = actualBuyOrderResponse.BuyOrderID;
        //Assert
        actualBuyOrderResponse.Should().NotBeNull();
        actualBuyOrderResponse.BuyOrderID.Should().NotBeEmpty();
        actualBuyOrderResponse.Should().Be(expectedBuyOrderResponse);
    }
    #endregion

    #region CreateSellOrder

    [Fact]
    // When you supply SellOrderRequest as null, it should throw ArgumentNullException.
    public async Task CreateSellOrder_NullSellOrderRequest_ToThrowArgumentNullException()
    {
        //Arrange
        SellOrderRequest sellOrderRequest = null;
        //Act
        Func<Task> action = async () =>
        {
            await _service.CreateSellOrder(sellOrderRequest);
        };
        //Assert
        await action.Should().ThrowAsync<ArgumentNullException>();
    }
    
    [Fact]
    // When you supply sellOrderQuantity as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
    public async Task CreateSellOrder_BelowMinQunatitySellOrder_ToThrowArgumentException()
    {
        //Arrange
        SellOrderRequest sellOrderRequest = _fixture.Build<SellOrderRequest>()
            .With(temp=>temp.Quantity,0)
            .With(temp => temp.DateAndTimeOfOrder, DateTime.Today).Create();
        //Act
        Func<Task> action = async () =>
        {
            await _service.CreateSellOrder(sellOrderRequest);
        };
        //Assert
        await action.Should().ThrowAsync<ArgumentException>();
    }
    [Fact]
    // When you supply sellOrderQuantity as 100001 (as per the specification, maximum is 100000), it should throw ArgumentException.
    public async Task CreateSellOrder_AboveMaxQuantitybuyOrder_ToThrowArgumentException()
    {
        //Arrange
        SellOrderRequest sellOrderRequest = _fixture.Build<SellOrderRequest>()
            .With(temp=>temp.Quantity,100001)
            .With(temp => temp.DateAndTimeOfOrder, DateTime.Today).Create();
        //Act
        Func<Task> action = async () =>
        {
            await _service.CreateSellOrder(sellOrderRequest);
        };
        //Assert
        await action.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    // When you supply sellOrderPrice as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
    public async Task CreateSellOrder_BelowMinPriceSellOrder_ToThrowArgumentException()
    {
        //Arrange
        SellOrderRequest sellOrderRequest = _fixture.Build<SellOrderRequest>()
            .With(temp=>temp.Price,0)
            .With(temp => temp.DateAndTimeOfOrder, DateTime.Today).Create();
        //Act
        Func<Task> action = async () =>
        {
            await _service.CreateSellOrder(sellOrderRequest);
        };
        //Assert
        await action.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    // When you supply sellOrderPrice as 10001 (as per the specification, maximum is 10000), it should throw ArgumentException.
    public async Task CreateSellOrder_AboveMaxSellOrder_ToThrowArgumentException()
    {
        //Arrange
        SellOrderRequest sellOrderRequest = _fixture.Build<SellOrderRequest>()
            .With(temp=>temp.Price,10001)
            .With(temp => temp.DateAndTimeOfOrder, DateTime.Today).Create();
        //Act
        Func<Task> action = async () =>
        {
            await _service.CreateSellOrder(sellOrderRequest);
        };
        //Assert
        await action.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    // When you supply stock symbol=null (as per the specification, stock symbol can't be null), it should throw ArgumentException.
    public async Task CreateSellOrder_NullorEmptyStockSymbol_ToThrowArgumentNullException()
    {
        //Arrange
        SellOrderRequest sellOrderRequest = _fixture.Build<SellOrderRequest>()
            .With(temp=>temp.StockSymbol,null as string)
            .With(temp => temp.DateAndTimeOfOrder, DateTime.Today).Create();
        //Act
        Func<Task> action = async () =>
        {
            await _service.CreateSellOrder(sellOrderRequest);
        };
        //Assert
        await action.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    // When you supply dateAndTimeOfOrder as "1999-12-31" (YYYY-MM-DD) - (as per the specification, it should be equal or newer date than 2000-01-01), it should throw ArgumentException.
    public async Task CreateSellOrder_MinDateofSellOrder_ToThrowArgumentException()
    {
        //Arrange
        SellOrderRequest sellOrderRequest = _fixture.Build<SellOrderRequest>()
            .With(temp => temp.DateAndTimeOfOrder, DateTime.Parse("1999-12-30")).Create();
        //Act
        Func<Task> action = async () =>
        {
            await _service.CreateSellOrder(sellOrderRequest);
        };
        //Assert
        await action.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    //If you supply all valid values, it should be successful and return an object of SellOrderResponse type with auto-generated SellOrderID (guid).
    public async void CreateSellOrder_ValidValue_ToBeSuccessful()
    {
        //Arrange
        SellOrderRequest sellOrderRequest = _fixture.Build<SellOrderRequest>()
            .With(temp=>temp.DateAndTimeOfOrder, DateTime.Today).Create();
        SellOrder sellOrder = sellOrderRequest.toSellOrder();
        SellOrderResponse expectedSellOrderResponse = sellOrder.ToSellOrderResponse();
        _repositoryMock.Setup(temp=>
            temp.CreateSellOrder(It.IsAny<SellOrder>())).ReturnsAsync(sellOrder);
        //Act
        SellOrderResponse? actualSellOrderResponse =  await _service.CreateSellOrder(sellOrderRequest);
        expectedSellOrderResponse.SellOrderID = actualSellOrderResponse.SellOrderID;
        //Assert
        actualSellOrderResponse.Should().NotBeNull();
        actualSellOrderResponse.SellOrderID.Should().NotBeEmpty();
        actualSellOrderResponse.Should().Be(expectedSellOrderResponse);
    }
    #endregion

    #region GetBuyOrders

    [Fact]
    // When you invoke this method, by default, the returned list should be empty.
    public async Task GetBuyOrders_EmptyList()
    {
        //Arange
        List<BuyOrder> buyOrders = new List<BuyOrder>();
        List<BuyOrderResponse> expectedResponse = buyOrders.Select(temp=>temp.toBuyOrderResponse()).ToList();
        _repositoryMock.Setup(temp=>
            temp.GetBuyOrders()).ReturnsAsync(buyOrders);
        //Act
        List<BuyOrderResponse> actualResponse = await _service.GetBuyOrders();
        //Assert
        actualResponse.Should().BeEquivalentTo(expectedResponse);
    }

    [Fact]
    // When you first add few buy orders using CreateBuyOrder() method; and then invoke GetAllBuyOrders() method; the returned list should contain all the same buy orders.
    public async Task GetBuyOrders_ValidList()
    {
        //Arrange
        List<BuyOrder> buyOrders = new List<BuyOrder>()
        {
            _fixture.Build<BuyOrder>()
                .With(temp => temp.DateAndTimeOfOrder, DateTime.Today).Create(),
            _fixture.Build<BuyOrder>()
                .With(temp => temp.DateAndTimeOfOrder, DateTime.Today).Create()
        };
        _repositoryMock.Setup(temp=>
            temp.GetBuyOrders()).ReturnsAsync(buyOrders);
        List<BuyOrderResponse> expectedResponse = buyOrders.Select(temp=>temp.toBuyOrderResponse()).ToList();
        //Act
        List<BuyOrderResponse> actualResponse = await _service.GetBuyOrders();
        //Assert
        actualResponse.Should().BeEquivalentTo(expectedResponse);
    }
    #endregion

    #region GetSellOrders

    [Fact]
    // When you invoke this method, by default, the returned list should be empty.
    public async Task GetSellOrders_EmptyList()
    {
        //Arange
        List<SellOrder> sellOrders = new List<SellOrder>();
        List<SellOrderResponse> expectedResponse = sellOrders.Select(temp=>temp.ToSellOrderResponse()).ToList();
        _repositoryMock.Setup(temp=>
            temp.GetSellOrders()).ReturnsAsync(sellOrders);
        //Act
        List<SellOrderResponse> actualResponse = await _service.GetSellOrders();
        //Assert
        actualResponse.Should().BeEquivalentTo(expectedResponse);
    }

    [Fact]
    // When you first add few buy orders using CreateSellOrder() method; and then invoke GetAllSellOrders() method; the returned list should contain all the same buy orders.
    public async void GetSellOrders_ValidList()
    {
        //Arrange
        List<SellOrder> sellOrders = new List<SellOrder>()
        {
            _fixture.Build<SellOrder>()
                .With(temp => temp.DateAndTimeOfOrder, DateTime.Today).Create(),
            _fixture.Build<SellOrder>()
                .With(temp => temp.DateAndTimeOfOrder, DateTime.Today).Create()
        };
        _repositoryMock.Setup(temp=>
            temp.GetSellOrders()).ReturnsAsync(sellOrders);
        List<SellOrderResponse> expectedResponse = sellOrders.Select(temp=>temp.ToSellOrderResponse()).ToList();
        //Act
        List<SellOrderResponse> actualResponse = await _service.GetSellOrders();
        //Assert
        actualResponse.Should().BeEquivalentTo(expectedResponse);
    }
    #endregion

}