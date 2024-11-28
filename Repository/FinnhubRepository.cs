using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RepositoryContracts;
using Operation = SerilogTimings.Operation;

namespace Repository;

public class FinnhubRepository(
    IHttpClientFactory httpClientFactory,
    IConfiguration configuration,
    ILogger<FinnhubRepository> logger)
    : IFinnhubRepository
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient();

    //create http client

    public async Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol)
    {
        logger.LogInformation($"Getting company profile for {stockSymbol}");
        //create http request
        HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"https://finnhub.io/api/v1/stock/profile2?symbol={stockSymbol}&token={configuration["FinnhubToken"]}") //URI includes the secret token
        };
        
        HttpResponseMessage httpResponseMessage;
        using (Operation.Time("Time for getting company profile"))
        {
            //send request
            httpResponseMessage = await _httpClient.SendAsync(httpRequestMessage);   
        }

        //read response body
        string responseBody = await new StreamReader(await httpResponseMessage.Content.ReadAsStreamAsync()).ReadToEndAsync();

        //convert response body (from JSON into Dictionary)
        Dictionary<string, object>? responseDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(responseBody);
        return responseDictionary;
    }

    public async Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol)
    {
        logger.LogInformation("Getting stock price quote for " + stockSymbol + "");
        //create http request
        HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"https://finnhub.io/api/v1/quote?symbol={stockSymbol}&token={configuration["FinnhubToken"]}") //URI includes the secret token
        };
        
        HttpResponseMessage httpResponseMessage;
        using (Operation.Time("Time for getting stock price quote"))
        {
            //send request
            httpResponseMessage = await _httpClient.SendAsync(httpRequestMessage);   
        }

        //read response body
        string responseBody = await new StreamReader(await httpResponseMessage.Content.ReadAsStreamAsync()).ReadToEndAsync();

        //convert response body (from JSON into Dictionary)
        Dictionary<string, object>? responseDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(responseBody);
        return responseDictionary;
    }

    public async Task<List<Dictionary<string, string>>?> GetStocks()
    {
        logger.LogInformation("Getting list of stocks");
        //create http request
        HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"https://finnhub.io/api/v1/stock/symbol?exchange=US&token={configuration["FinnhubToken"]}") //URI includes the secret token
        }; 
        
        _httpClient.Timeout = TimeSpan.FromMinutes(6);
        HttpResponseMessage httpResponseMessage;
        using (Operation.Time("Time for getting all stocks"))
        {
            //send request
            httpResponseMessage = await _httpClient.SendAsync(httpRequestMessage);   
        }
        //read response body
        string response = await httpResponseMessage.Content.ReadAsStringAsync();
        
        //convert response body (from JSON into Dictionary)
        List<Dictionary<string, string>>? responseDictionary =  JsonSerializer.Deserialize<List<Dictionary<string, string>>?>(response);
        if (responseDictionary == null)
            return null;
        return responseDictionary;
    }
    
    public async Task<Dictionary<string, object>?> SearchStocks(string stockSymbolToSearch)
    {
        logger.LogInformation($"Searching for the stock {stockSymbolToSearch}");
        //create http request
        HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"https://finnhub.io/api/v1/search?q={stockSymbolToSearch}&token={configuration["FinnhubToken"]}") //URI includes the secret token
        };
        
        HttpResponseMessage httpResponseMessage;
        using (Operation.Time("Time for searching for a stock"))
        {
            //send request
            httpResponseMessage = await _httpClient.SendAsync(httpRequestMessage);   
        }

        //read response body
        string responseBody = await  new StreamReader(await httpResponseMessage.Content.ReadAsStreamAsync()).ReadToEndAsync();
        //convert response body (from JSON into Dictionary)
        Dictionary<string, object>? responseDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(responseBody);
        if (responseDictionary != null && responseDictionary.TryGetValue("result", out var value))
        {
            var x = value.ToString();
            List<Dictionary<string, object>>? result = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(x!);
            return result?[0];
        }
        return responseDictionary;
    }
}