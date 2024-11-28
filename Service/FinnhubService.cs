using Microsoft.Extensions.Logging;
using RepositoryContracts;
using ServiceContract;

namespace Service;
public class FinnhubService(IFinnhubRepository finnhubRepository, ILogger<FinnhubService> logger)
    : IFinnhubService
{
    public async Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol)
    {
        try
        {
            Dictionary<string, object>? responseDictionary = await finnhubRepository.GetCompanyProfile(stockSymbol);
            if (responseDictionary == null)
            {
                logger.LogError("No response received from Finnhub API");
                throw new InvalidOperationException("No response from server");   
            }
            if (responseDictionary.TryGetValue("error", out var value))
            {
                var errorMessage = Convert.ToString(value);
                logger.LogError($"API returned error: {errorMessage}");
                throw new InvalidOperationException(errorMessage);
            }
            return responseDictionary;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error occured while getting company profile");
            throw;
        }
    }
    
    public async Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol)
    {
        try
        {
            Dictionary<string, object>? responseDictionary = await finnhubRepository.GetStockPriceQuote(stockSymbol);
            if (responseDictionary == null)
            {
                logger.LogError("No response received from Finnhub API");
                throw new InvalidOperationException("No response from server");   
            }
            if (responseDictionary.TryGetValue("error", out var value))
            {
                var errorMessage = Convert.ToString(value);
                logger.LogError($"API returned error: {errorMessage}");
                throw new InvalidOperationException(errorMessage);
            }
            //return response dictionary back to the caller
            return responseDictionary;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error occured while getting stock price quote");
            throw;
        }
    }

    public async Task<List<Dictionary<string, string>>?> GetStocks()
    {
        try
        {
            List<Dictionary<string,string>>? responseList = await finnhubRepository.GetStocks();  
            if (responseList == null)
            {
                logger.LogError("No response received from Finnhub API");
                throw new InvalidOperationException("No response from server");   
            }
            Dictionary<string, string>? error = responseList.FirstOrDefault( temp => temp.ContainsKey("error"));
            if (error != null)
            {
                string errorMessage = Convert.ToString(error["error"]);
                logger.LogError($"API returned error: {errorMessage}");
                throw new InvalidOperationException(errorMessage);
            }
            return responseList;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error occured while fetching stocks");
            throw;
        }
    }

    public async Task<Dictionary<string, object>?> SearchStocks(string stockSymbolToSearch)
    {
        try
        {
            Dictionary<string, object>? responseDictionary = await finnhubRepository.SearchStocks(stockSymbolToSearch);
            if (responseDictionary == null)
            {
                logger.LogError("No response received from Finnhub API");
                throw new InvalidOperationException("No response from server");    
            }
            if (responseDictionary.TryGetValue("error", out var value))
            {
                var errorMessage = Convert.ToString(value);
                logger.LogError($"API returned error: {errorMessage}");
                throw new InvalidOperationException(errorMessage);   
            }
            return responseDictionary;
        }
        catch (Exception e)
        { 
            logger.LogError(e,$"Error occured while searching for stock {stockSymbolToSearch}");
            throw;
        }
    }
}