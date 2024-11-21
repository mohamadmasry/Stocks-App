using ServiceContract.DTO;

namespace StocksApp.Models;

public class Orders
{
    public List<SellOrderResponse> SellOrderResponses { get; set; } 
    public List<BuyOrderResponse> BuyOrderResponses { get; set; } 

}