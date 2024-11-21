using ServiceContract.DTO;

namespace ServiceContract;

public interface IStocksService
{
    /// <summary>
    /// Creates a buy order with fields filled from the BuyOrderRequest argument object and returns BuyOrderResponse
    /// </summary>
    /// <param name="buyOrderRequest">The DTO object of BuyOrder containing the buy order information</param>
    /// <returns>The DTO object of BuyOrder returned to controller</returns>
    Task<BuyOrderResponse?> CreateBuyOrder(BuyOrderRequest? buyOrderRequest);

    /// <summary>
    /// Creates a sell order with fields filled from the SellOrderRequest argument object and returns SellOrderResponse
    /// </summary>
    /// <param name="buyOrderRequest">The DTO object of SellOrder containing the buy order information</param>
    /// <returns>The DTO object of SellOrder returned to controller</returns>
    Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest);
    
    /// <summary>
    /// Returns all buy orders as a list of BuylOrderResponse
    /// </summary>
    /// <returns>List of DTO object of BuyOrder</returns>
    Task<List<BuyOrderResponse>> GetBuyOrders();
    
    /// <summary>
    /// Returns all sell orders as a list of SellOrderResponse
    /// </summary>
    /// <returns>List of DTO object of SellOrder</returns>
    Task<List<SellOrderResponse>> GetSellOrders();
}