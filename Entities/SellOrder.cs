using System.ComponentModel.DataAnnotations;

namespace Entities;

public class SellOrder
{
    [Key]
    public Guid SellOrderID {get;set;}
    
    [Required]
    public string StockSymbol{get;set;}

    [Required] 
    public string StockName{get;set;}
    
    [DataType(DataType.DateTime)]
    public DateTime DateAndTimeOfOrder { get; set; }
    
    [Range(0,100000)] 
    public int Quantity{get;set;}
    [Range(0,10000)]
    public double Price{get;set;}

}