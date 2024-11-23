namespace ServiceContract;

public interface IFinnhubService
{
    /// <summary>
    /// Gets the profile of a stock's company from Finnhub.io acording the stockSymol  
    /// </summary>
    /// <param name="stockSymbol">Specifies which stock</param>
    /// <returns>A dictionary containing the company profile information</returns>
    Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol);

    /// <summary>
    /// Gets the price quote of a stock from Finnhub.io acording the stockSymol  
    /// </summary>
    /// <param name="stockSymbol">Specifies which stock</param>
    /// <returns>A dictionary containing the stock price quote information</returns>
    Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol);

    /// <summary>
    /// Gets stock information from Finnhub.io
    /// </summary>
    /// <returns>A list of dictionaries containing information about the stocks </returns>
    Task<List<Dictionary<string, string>>?> GetStocks();

    /// <summary>
    /// Searches for a specific stock according to stockSymbolToSearch 
    /// </summary>
    /// <param name="stockSymbolToSearch">Specifies which stock</param>
    /// <returns>A dictionary containing the stock info</returns>
    Task<Dictionary<string, object>?> SearchStocks(string stockSymbolToSearch);


}