using Entities;
using Microsoft.Extensions.Logging;
using RepositoryContracts;
using Service.Helpers;
using ServiceContract;
using ServiceContract.DTO;

namespace Service;

public class StocksService(IStocksRepository stocksRepository, ILogger<StocksService> logger)
    : IStocksService
{
    
    public async Task<BuyOrderResponse?> CreateBuyOrder(BuyOrderRequest? buyOrderRequest)
    {
        try
        {
            //Check if buyOrderRequest is null
            if (buyOrderRequest == null)
            {
                logger.LogError("Null buy order request");
                throw new ArgumentNullException(nameof(buyOrderRequest));   
            }
            //Model Validation
            ValidationHelper.ModelValidation(buyOrderRequest);
            //Convert buyOrderRequest to BuyOrder
            BuyOrder buyOrder = buyOrderRequest.toBuyOrder();
            buyOrder.BuyOrderID = Guid.NewGuid();
            //Add buyOrder to List
            await stocksRepository.CreateBuyOrder(buyOrder);
            //Return BuyOrderResponse
            return buyOrder.toBuyOrderResponse();
        }
        catch (Exception e)
        {
            logger.LogError(e,"Error occured while creating buy order");
            throw;
        }
    }
    public async Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest)
    {
        try
        {
            //Check if buyOrderRequest is null
            if(sellOrderRequest == null)
            {
                logger.LogError("Null sell order request");
                throw new ArgumentNullException(nameof(sellOrderRequest));
            }
            //Model Validation
            ValidationHelper.ModelValidation(sellOrderRequest);
            //Convert buyOrderRequest to BuyOrder
            SellOrder sellOrder = sellOrderRequest.toSellOrder();
            sellOrder.SellOrderID = Guid.NewGuid();
            //Add buyOrder to List
            await stocksRepository.CreateSellOrder(sellOrder);
            //Return BuyOrderResponse
            return sellOrder.ToSellOrderResponse();
        }
        catch (Exception e)
        {
            logger.LogError(e,"Error occured while creating sell order");
            throw;
        }
    }
    public async Task<List<BuyOrderResponse>> GetBuyOrders()
    {
        try
        {
            var buyOrders = await stocksRepository.GetBuyOrders(); 
            return buyOrders.Select(buyOrder => buyOrder.toBuyOrderResponse()).ToList();
        }
        catch (Exception e)
        {
            logger.LogError(e,"Error occured while fetching buy orders");
            throw;
        }
    }
    public async Task<List<SellOrderResponse>> GetSellOrders()
    {
        try
        {
            List<SellOrder> sellOrders = await stocksRepository.GetSellOrders();
            return sellOrders.Select(sellOrder => sellOrder.ToSellOrderResponse()).ToList();
        }
        catch (Exception e)
        {
            logger.LogError(e,"Error occured while fetching sell orders");
            throw;
        }
    }
}