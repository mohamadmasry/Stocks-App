using System.ComponentModel.DataAnnotations;
using Entities;

namespace ServiceContract.DTO;

public class SellOrderResponse
{
    public Guid SellOrderID{ get; set; }
    
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
        if(obj == null)
            return false;
        SellOrderResponse other = obj as SellOrderResponse;
        return other.StockName.Equals(StockName) && 
               other.Price == Price &&
               other.Quantity == Quantity &&
               other.StockSymbol.Equals(StockSymbol) &&
               other.DateAndTimeOfOrder.Equals(DateAndTimeOfOrder) &&
               other.TradeAmount == TradeAmount &&
               other.SellOrderID.Equals(SellOrderID);
    }
}
public static class SellOrderExtensions
{    
    /// <summary>
    /// Extension method to convert a SellOrder object to SellOrderResponse 
    /// </summary>
    /// <param name="person">The SellOrder object to convert</param>
    /// <returns>The converted SellOrderResponse</returns>
    public static SellOrderResponse ToSellOrderResponse(this SellOrder sellOrder)
    {
        return new SellOrderResponse()
        {
            StockSymbol = sellOrder.StockSymbol,
            Price = sellOrder.Price,
            Quantity = sellOrder.Quantity,
            StockName = sellOrder.StockName,
            DateAndTimeOfOrder = sellOrder.DateAndTimeOfOrder,
            SellOrderID = sellOrder.SellOrderID,
            TradeAmount = sellOrder.Quantity * sellOrder.Price
        };
    }
}