using System.ComponentModel.DataAnnotations;
using Entities;

namespace ServiceContract.DTO;

public class BuyOrderRequest
{
    [Required]
    public string StockSymbol{get;set;}

    [Required]
    public string StockName{get;set;}
    
    [DataType(DataType.DateTime)]
    [Range(typeof(DateTime),"01/01/2000","31/12/2025" ,ErrorMessage = "Minimum order year is 2000")]
    public DateTime DateAndTimeOfOrder { get; set; }
    [Required]
    [Range(1,100000,ErrorMessage = "Minimum order quantity is 1")]
    public int Quantity{get;set;}
    
    [Range(1,10000)]
    public double Price{get;set;}

    public BuyOrder toBuyOrder()
    {
        return new BuyOrder()
        {
            StockSymbol = StockSymbol,
            DateAndTimeOfOrder = DateAndTimeOfOrder,
            Price = Price,
            Quantity = Quantity,
            StockName = StockName
        };
    }
}