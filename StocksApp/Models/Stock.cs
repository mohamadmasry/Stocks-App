namespace StocksApp.Models;

public class Stock
{
    public string? StockSymbol {get;set;}

    public string? StockName {get;set;}

    public override string ToString()
    {
        return $"Stock Symbol: {StockSymbol}, Stock Name: {StockName}";
    }
}