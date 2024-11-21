using Entities;
using Microsoft.EntityFrameworkCore;
using Service;
using ServiceContract;
using ServiceContract.DTO;

namespace StocksAppTest;

public class IStocksServiceTest
{
    private readonly IStocksService _service;

    #region CreateBuyOrder
    
    [Fact]
    // When you supply BuyOrderRequest as null, it should throw ArgumentNullException.
    public void CreateBuyOrder_NullBuyOrderRequest()
    {
        //Arrange
        BuyOrderRequest buyOrderRequest = null;
        //Assert
        Assert.ThrowsAsync<ArgumentNullException>(() =>
            //Act
            _service.CreateBuyOrder(buyOrderRequest));
    }

    [Fact]
    // When you supply buyOrderQuantity as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
    public void CreateBuyOrder_buyOrderQunatityMin()
    {
        //Arrange
        BuyOrderRequest buyOrderRequest = new BuyOrderRequest()
        {
            DateAndTimeOfOrder = DateTime.Parse("2021-01-01"),
            Quantity = 0,
            Price = 100,
            StockName = "Apple",
            StockSymbol = "AAPL"
        };
        //Assert
        Assert.ThrowsAsync<ArgumentException>(() =>
            //Act
            _service.CreateBuyOrder(buyOrderRequest));
    }

    [Fact]
    // When you supply buyOrderQuantity as 100001 (as per the specification, maximum is 100000), it should throw ArgumentException.
    public void CreateBuyOrder_buyOrderQuantityMax()
    {
        //Arrange
        BuyOrderRequest buyOrderRequest = new BuyOrderRequest()
        {
            DateAndTimeOfOrder = DateTime.Parse("2021-01-01"),
            Quantity = 100001,
            Price = 0,
            StockName = "Apple",
            StockSymbol = "AAPL"
        };
        //Assert
        Assert.ThrowsAsync<ArgumentException>(() =>
            //Act
            _service.CreateBuyOrder(buyOrderRequest));
    }

    [Fact]
    // When you supply buyOrderPrice as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
    public void CreateBuyOrder_buyOrderPriceMin()
    {
        //Arrange
        BuyOrderRequest buyOrderRequest = new BuyOrderRequest()
        {
            DateAndTimeOfOrder = DateTime.Parse("2021-01-01"),
            Quantity = 1000,
            Price = 0,
            StockName = "Apple",
            StockSymbol = "AAPL"
        };
        //Assert
        Assert.ThrowsAsync<ArgumentException>(() =>
            //Act
            _service.CreateBuyOrder(buyOrderRequest));
    }

    [Fact]
    // When you supply buyOrderPrice as 10001 (as per the specification, maximum is 10000), it should throw ArgumentException.
    public void CreateBuyOrder_buyOrderPriceMax()
    {
        //Arrange
        BuyOrderRequest buyOrderRequest = new BuyOrderRequest()
        {
            DateAndTimeOfOrder = DateTime.Parse("2021-01-01"),
            Quantity = 1000,
            Price = 10001,
            StockName = "Apple",
            StockSymbol = "AAPL"
        };
        //Assert
        Assert.ThrowsAsync<ArgumentException>(() =>
            //Act
            _service.CreateBuyOrder(buyOrderRequest));
    }

    [Fact]
    // When you supply stock symbol=null (as per the specification, stock symbol can't be null), it should throw ArgumentException.
    public void CreateBuyOrder_stockSymbolNullorEmpty()
    {
        //Arrange
        BuyOrderRequest buyOrderRequest = new BuyOrderRequest()
        {
            DateAndTimeOfOrder = DateTime.Parse("2021-01-01"),
            Quantity = 1000,
            Price = 100,
            StockName = "Apple",
        };
        //Assert
        Assert.ThrowsAsync<ArgumentException>(() =>
            //Act
            _service.CreateBuyOrder(buyOrderRequest));
    }

    [Fact]
    // When you supply dateAndTimeOfOrder as "1999-12-31" (YYYY-MM-DD) - (as per the specification, it should be equal or newer date than 2000-01-01), it should throw ArgumentException.
    public void CreateBuyOrder_DateofOrderMin()
    {
        //Arrange
        BuyOrderRequest buyOrderRequest = new BuyOrderRequest()
        {
            DateAndTimeOfOrder = DateTime.Parse("1999-12-20"),
            Quantity = 1000,
            Price = 100,
            StockSymbol = "AAPL",
            StockName = "Apple",
        };
        //Assert
        Assert.ThrowsAsync<ArgumentException>(() =>
            //Act
            _service.CreateBuyOrder(buyOrderRequest));
    }

    [Fact]
    //If you supply all valid values, it should be successful and return an object of BuyOrderResponse type with auto-generated BuyOrderID (guid).
    public async void CreateBuyOrder_ValidValue()
    {
        //Arrange
        BuyOrderRequest buyOrderRequest = new BuyOrderRequest()
        {
            DateAndTimeOfOrder = DateTime.Parse("2002-9-15"),
            Quantity = 1000,
            Price = 100,
            StockSymbol = "AAPL",
            StockName = "Apple",
        };
        //Act
        BuyOrderResponse? buyOrderResponse =  await _service.CreateBuyOrder(buyOrderRequest);
        //Assert
        Assert.Contains(buyOrderResponse, await _service.GetBuyOrders());
        Assert.True(buyOrderResponse?.BuyOrderID != Guid.Empty);
    }
    #endregion

    #region CreateSellOrder

    [Fact]
    // When you supply SellOrderRequest as null, it should throw ArgumentNullException.
    public void CreateSellOrder_NullSellOrderRequest()
    {
        //Arrange
        SellOrderRequest sellOrderRequest = null;
        //Assert
        Assert.ThrowsAsync<ArgumentNullException>(() =>
            //Act
            _service.CreateSellOrder(sellOrderRequest));
    }

    [Fact]
    // When you supply sellOrderQuantity as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
    public void CreateSellOrder_sellOrderQunatityMin()
    {
        //Arrange
        SellOrderRequest sellOrderRequest = new SellOrderRequest()
        {
            DateAndTimeOfOrder = DateTime.Parse("2021-01-01"),
            Quantity = 0,
            Price = 100,
            StockName = "Apple",
            StockSymbol = "AAPL"
        };
        //Assert
        Assert.ThrowsAsync<ArgumentException>(() =>
            //Act
            _service.CreateSellOrder(sellOrderRequest));
    }
    [Fact]
    // When you supply sellOrderQuantity as 100001 (as per the specification, maximum is 100000), it should throw ArgumentException.
    public void CreateSellOrder_buyOrderQuantityMax()
    {
        //Arrange
        SellOrderRequest sellOrderRequest = new SellOrderRequest()
        {
            DateAndTimeOfOrder = DateTime.Parse("2021-01-01"),
            Quantity = 100001,
            Price = 0,
            StockName = "Apple",
            StockSymbol = "AAPL"
        };
        //Assert
        Assert.ThrowsAsync<ArgumentException>(() =>
            //Act
            _service.CreateSellOrder(sellOrderRequest));
    }

    [Fact]
    // When you supply sellOrderPrice as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
    public void CreateSellOrder_sellOrderPriceMin()
    {
        //Arrange
        SellOrderRequest sellOrderRequest = new SellOrderRequest()
        {
            DateAndTimeOfOrder = DateTime.Parse("2021-01-01"),
            Quantity = 1000,
            Price = 0,
            StockName = "Apple",
            StockSymbol = "AAPL"
        };
        //Assert
        Assert.ThrowsAsync<ArgumentException>(() =>
            //Act
            _service.CreateSellOrder(sellOrderRequest));
    }

    [Fact]
    // When you supply sellOrderPrice as 10001 (as per the specification, maximum is 10000), it should throw ArgumentException.
    public void CreateSellOrder_sellOrderPriceMax()
    {
        //Arrange
        SellOrderRequest sellOrderRequest = new SellOrderRequest()
        {
            DateAndTimeOfOrder = DateTime.Parse("2021-01-01"),
            Quantity = 1000,
            Price = 10001,
            StockName = "Apple",
            StockSymbol = "AAPL"
        };
        //Assert
        Assert.ThrowsAsync<ArgumentException>(() =>
            //Act
            _service.CreateSellOrder(sellOrderRequest));
    }

    [Fact]
    // When you supply stock symbol=null (as per the specification, stock symbol can't be null), it should throw ArgumentException.
    public void CreateSellOrder_stockSymbolNullorEmpty()
    {
        //Arrange
        SellOrderRequest sellOrderRequest = new SellOrderRequest()
        {
            DateAndTimeOfOrder = DateTime.Parse("2021-01-01"),
            Quantity = 1000,
            Price = 100,
            StockName = "Apple",
        };
        //Assert
        Assert.ThrowsAsync<ArgumentException>(() =>
            //Act
            _service.CreateSellOrder(sellOrderRequest));
    }

    [Fact]
    // When you supply dateAndTimeOfOrder as "1999-12-31" (YYYY-MM-DD) - (as per the specification, it should be equal or newer date than 2000-01-01), it should throw ArgumentException.
    public void CreateSellOrder_DateofOrderMin()
    {
        //Arrange
        SellOrderRequest sellOrderRequest = new SellOrderRequest()
        {
            DateAndTimeOfOrder = DateTime.Parse("1999-12-20"),
            Quantity = 1000,
            Price = 100,
            StockSymbol = "AAPL",
            StockName = "Apple",
        };
        //Assert
        Assert.ThrowsAsync<ArgumentException>(() =>
            //Act
            _service.CreateSellOrder(sellOrderRequest));
    }

    [Fact]
    //If you supply all valid values, it should be successful and return an object of SellOrderResponse type with auto-generated SellOrderID (guid).
    public async void CreateSellOrder_ValidValue()
    {
        //Arrange
        SellOrderRequest sellOrderRequest = new SellOrderRequest()
        {
            DateAndTimeOfOrder = DateTime.Parse("2002-9-15"),
            Quantity = 1000,
            Price = 100,
            StockSymbol = "AAPL",
            StockName = "Apple",
        };
        //Act
        SellOrderResponse? sellOrderResponse =  await _service.CreateSellOrder(sellOrderRequest);
        //Assert
        Assert.Contains(sellOrderResponse, await _service.GetSellOrders());
        Assert.True(sellOrderResponse?.SellOrderID != Guid.Empty);
    }
    #endregion

    #region GetBuyOrders

    [Fact]
    // When you invoke this method, by default, the returned list should be empty.
    public async void GetBuyOrders_EmptyList()
    {
        //Arange
        List<BuyOrderResponse> response = await _service.GetBuyOrders();
        //Assert
        Assert.Empty(response);
    }

    [Fact]
    // When you first add few buy orders using CreateBuyOrder() method; and then invoke GetAllBuyOrders() method; the returned list should contain all the same buy orders.
    public async void GetBuyOrders_ValidList()
    {
        //Arrange
        BuyOrderRequest buyOrderRequest1 = new BuyOrderRequest()
        {
            DateAndTimeOfOrder = DateTime.Parse("2021-01-01"),
            Price = 100,
            Quantity = 1000,
            StockSymbol = "AAPL",
            StockName = "Apple",
        };
        BuyOrderRequest buyOrderRequest2 = new BuyOrderRequest()
        {
            DateAndTimeOfOrder = DateTime.Parse("2015-10-03"),
            Price = 69.49,
            Quantity = 302,
            StockSymbol = "MSFT",
            StockName = "Microsoft",
        };
        BuyOrderResponse buyOrderResponse1 = await _service.CreateBuyOrder(buyOrderRequest1);
        BuyOrderResponse buyOrderResponse2 = await _service.CreateBuyOrder(buyOrderRequest2);
        List<BuyOrderResponse> expected_response = new List<BuyOrderResponse>()
        {
            buyOrderResponse1,
            buyOrderResponse2
        };
        //Act
        List<BuyOrderResponse> actual_response = await _service.GetBuyOrders();
        //Assert
        Assert.Equal(expected_response, actual_response);
    }
    #endregion

    #region GetSellOrders

    [Fact]
    // When you invoke this method, by default, the returned list should be empty.
    public async void GetSellOrders_EmptyList()
    {
        //Arange
        List<SellOrderResponse> response = await _service.GetSellOrders();
        //Assert
        Assert.Empty(response);
    }

    [Fact]
    // When you first add few buy orders using CreateSellOrder() method; and then invoke GetAllSellOrders() method; the returned list should contain all the same buy orders.
    public async void GetSellOrders_ValidList()
    {
        //Arrange
        SellOrderRequest sellOrderRequest1 = new SellOrderRequest()
        {
            DateAndTimeOfOrder = DateTime.Parse("2021-01-01"),
            Price = 100,
            Quantity = 1000,
            StockSymbol = "AAPL",
            StockName = "Apple",
        };
        SellOrderRequest sellOrderRequest2 = new SellOrderRequest()
        {
            DateAndTimeOfOrder = DateTime.Parse("2015-10-03"),
            Price = 69.49,
            Quantity = 302,
            StockSymbol = "MSFT",
            StockName = "Microsoft",
        };
        SellOrderResponse sellOrderResponse1 = await _service.CreateSellOrder(sellOrderRequest1);
        SellOrderResponse sellOrderResponse2 = await _service.CreateSellOrder(sellOrderRequest2);
        List<SellOrderResponse> expected_response = new List<SellOrderResponse>()
        {
            sellOrderResponse1,
            sellOrderResponse2
        };
        //Act
        List<SellOrderResponse> actual_response = await _service.GetSellOrders();
        //Assert
        Assert.Equal(expected_response, actual_response);
    }
    #endregion

}