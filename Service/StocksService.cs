using System.ComponentModel;
using System.Diagnostics;
using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;
using Service.Helpers;
using ServiceContract;
using ServiceContract.DTO;

namespace Service;

public class StocksService : IStocksService
{
    private readonly IStocksRepository _stocksRepository;
    public StocksService(IStocksRepository stocksRepository)
    {
        _stocksRepository = stocksRepository;
    }
    public async Task<BuyOrderResponse?> CreateBuyOrder(BuyOrderRequest? buyOrderRequest)
    {
        //Check if buyOrderRequest is null
        if(buyOrderRequest == null) 
            throw new ArgumentNullException(nameof(buyOrderRequest));
        //Model Validation
        ValidationHelper.ModelValidation(buyOrderRequest);
        //Convert buyOrderRequest to BuyOrder
        BuyOrder buyOrder = buyOrderRequest.toBuyOrder();
        buyOrder.BuyOrderID = Guid.NewGuid();
        //Add buyOrder to List
        await _stocksRepository.CreateBuyOrder(buyOrder);
        //Return BuyOrderResponse
        return buyOrder.toBuyOrderResponse();
    }
    public async Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest)
    {
        //Check if buyOrderRequest is null
        if(sellOrderRequest == null) 
            throw new ArgumentNullException(nameof(sellOrderRequest));
        //Model Validation
        ValidationHelper.ModelValidation(sellOrderRequest);
        //Convert buyOrderRequest to BuyOrder
        SellOrder sellOrder = sellOrderRequest.toSellOrder();
        sellOrder.SellOrderID = Guid.NewGuid();
        //Add buyOrder to List
        await _stocksRepository.CreateSellOrder(sellOrder);
        //Return BuyOrderResponse
        return sellOrder.ToSellOrderResponse();

    }
    public async Task<List<BuyOrderResponse>> GetBuyOrders()
    {
        var buyOrders = await _stocksRepository.GetBuyOrders(); 
        return buyOrders.Select(buyOrder => buyOrder.toBuyOrderResponse()).ToList();
    }
    public async Task<List<SellOrderResponse>> GetSellOrders()
    {
        var sellOrders = await _stocksRepository.GetSellOrders();
        return sellOrders.Select(sellOrder => sellOrder.ToSellOrderResponse()).ToList();
    }
}