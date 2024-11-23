using Microsoft.Extensions.Configuration;
using System.Text.Json;
using RepositoryContracts;
using ServiceContract;

namespace Service;
public class FinnhubService : IFinnhubService 
{ 
    private readonly IFinnhubRepository _finnhubRepository;
    public FinnhubService(IFinnhubRepository finnhubRepository)
    { 
        _finnhubRepository = finnhubRepository;
    }
    
    public async Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol)
    { 
        Dictionary<string,object>? responseDictionary = await _finnhubRepository.GetCompanyProfile(stockSymbol);
        if (responseDictionary == null)
            throw new InvalidOperationException("No response from server");
        if (responseDictionary.ContainsKey("error")) 
            throw new InvalidOperationException(Convert.ToString(responseDictionary["error"]));
        return responseDictionary;
    }
    
    public async Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol)
    {
        Dictionary<string,object>? responseDictionary = await _finnhubRepository.GetStockPriceQuote(stockSymbol);
        if (responseDictionary == null) 
            throw new InvalidOperationException("No response from server"); 
        if (responseDictionary.ContainsKey("error")) 
            throw new InvalidOperationException(Convert.ToString(responseDictionary["error"])); 
        //return response dictionary back to the caller
        return responseDictionary;
    }

    public async Task<List<Dictionary<string, string>>?> GetStocks()
    {
        List<Dictionary<string, string>>? responseList = await _finnhubRepository.GetStocks();
        if (responseList == null || responseList.Count == 0)
            throw new InvalidOperationException("No response from server");
        if (responseList.FirstOrDefault(temp => temp.ContainsKey("error")) != null)
        {
            Dictionary<string,string> sx = responseList.First(temp => temp.ContainsKey("error"));
            throw new InvalidOperationException(sx["error"]);
        }
        return responseList;
    }

    public async Task<Dictionary<string, object>?> SearchStocks(string stockSymbolToSearch)
    {
        Dictionary<string,object>? responseDictionary = await _finnhubRepository.SearchStocks(stockSymbolToSearch);
        if(responseDictionary == null)
            throw new InvalidOperationException("No response from server");
        if(responseDictionary.ContainsKey("error"))
            throw new InvalidOperationException(Convert.ToString(responseDictionary["error"]));
        return responseDictionary;
    }
}