using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.JavaScript;
using Entities;

namespace ServiceContract.DTO;

public class BuyOrderResponse
{
    public Guid BuyOrderID{ get; set; }
    
    [Required]
    public string StockSymbol{get;set;}

    [Required]
    public string StockName{get;set;}
    
    [DataType(DataType.DateTime)]
    [Range(typeof(DateTime),"01/01/2000","31/12/2025" ,ErrorMessage = "Minimum order year is 2000")]
    public DateTime DateAndTimeOfOrder { get; set; }
    
    [Range(0,100000)] 
    public int Quantity{get;set;}
    
    [Range(0,10000)]
    public double Price{get;set;}
    
    public double TradeAmount {get;set;}

    public override bool Equals(object? obj)
    {
        if (obj == null)
        {
            return false;
        }
        BuyOrderResponse other =  obj as BuyOrderResponse;
        return other.StockName.Equals(StockName) && 
               other.StockSymbol.Equals(StockSymbol) && 
               other.DateAndTimeOfOrder.Equals(DateAndTimeOfOrder) && 
               other.Quantity.Equals(Quantity) && 
               other.Price.Equals(Price) &&
               other.BuyOrderID.Equals(BuyOrderID) && 
               other.TradeAmount.Equals(TradeAmount);
    }
}

public static class BuyOrderExtensions
{    
    /// <summary>
    /// Extension method to convert a BuyOrder object to BuyOrderResponse 
    /// </summary>
    /// <param name="person">The BuyOrder object to convert</param>
    /// <returns>The converted BuyOrderResponse</returns>
    public static BuyOrderResponse toBuyOrderResponse(this BuyOrder buyOrder)
    {
        return new BuyOrderResponse()
        {
            StockSymbol = buyOrder.StockSymbol,
            Price = buyOrder.Price,
            Quantity = buyOrder.Quantity,
            StockName = buyOrder.StockName,
            DateAndTimeOfOrder = buyOrder.DateAndTimeOfOrder,
            BuyOrderID = buyOrder.BuyOrderID,
            TradeAmount = buyOrder.Quantity * buyOrder.Price
        };
    }
}