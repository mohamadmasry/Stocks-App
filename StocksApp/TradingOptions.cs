namespace StocksApp;

public class TradingOptions
{
    public string? Top25PopularStocks { get; set; }
    public string? DefaultOrderQuantity { get; set; }

    public List<string>? GetStocks()
    {
        if (String.IsNullOrEmpty(Top25PopularStocks)) 
            return null;
        List<string> stocks = Top25PopularStocks.Split(",").ToList();
        return stocks;
    }
    
}