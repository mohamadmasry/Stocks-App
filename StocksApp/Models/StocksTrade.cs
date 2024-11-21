namespace StocksApp.Models;

public class StocksTrade
{
    public string? StockSymbol { get; set; }

    public string? StockName { get; set; }

    public double Price { get; set; }

    public int Quantity { get; set; }
}