namespace ServiceContract;

public interface IFinnhubService
{
    /// <summary>
    /// Gets the profile of a stock's company from Finnhub.io acording the stockSymol  
    /// </summary>
    /// <param name="stockSymbol">Specifies which stock</param>
    /// <returns>A dictionary containing the company profile information</returns>
    Dictionary<string, object>? GetCompanyProfile(string stockSymbol);

    /// <summary>
    /// Gets the price quote of a stock from Finnhub.io acording the stockSymol  
    /// </summary>
    /// <param name="stockSymbol">Specifies which stock</param>
    /// <returns>A dictionary containing the stock price quote information</returns>
    Dictionary<string, object>? GetStockPriceQuote(string stockSymbol);
}