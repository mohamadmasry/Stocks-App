using System.Configuration;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using RepositoryContracts;

namespace Repository;

public class FinnhubRepository : IFinnhubRepository
{
    private readonly HttpClient httpClient;
    private readonly IConfiguration _configuration;

    public FinnhubRepository(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        //create http client
        httpClient = httpClientFactory.CreateClient();
        _configuration = configuration;
    }
    public async Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol)
    {
        //create http request
        HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"https://finnhub.io/api/v1/stock/profile2?symbol={stockSymbol}&token={_configuration["FinnhubToken"]}") //URI includes the secret token
        };

        //send request
        HttpResponseMessage httpResponseMessage = httpClient.Send(httpRequestMessage);

        //read response body
        string responseBody = new StreamReader(httpResponseMessage.Content.ReadAsStream()).ReadToEnd();

        //convert response body (from JSON into Dictionary)
        Dictionary<string, object>? responseDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(responseBody);
        return responseDictionary;
    }

    public async Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol)
    {
        //create http request
        HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"https://finnhub.io/api/v1/quote?symbol={stockSymbol}&token={_configuration["FinnhubToken"]}") //URI includes the secret token
        };

        //send request
        HttpResponseMessage httpResponseMessage = httpClient.Send(httpRequestMessage);

        //read response body
        string responseBody = new StreamReader(httpResponseMessage.Content.ReadAsStream()).ReadToEnd();

        //convert response body (from JSON into Dictionary)
        Dictionary<string, object>? responseDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(responseBody);
        return responseDictionary;
    }

    public async Task<List<Dictionary<string, string>>?> GetStocks()
    {
        //create http request
        HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"https://finnhub.io/api/v1/stock/symbol?exchange=US&token={_configuration["FinnhubToken"]}") //URI includes the secret token
        };

        //send request
        HttpResponseMessage httpResponseMessage = httpClient.Send(httpRequestMessage);

        //read response body
        string responseBody = new StreamReader(httpResponseMessage.Content.ReadAsStream()).ReadToEnd();
        
        //convert response body (from JSON into Dictionary)
        List<Dictionary<string, string>>? responseDictionary = JsonSerializer.Deserialize<List<Dictionary<string, string>>?>(responseBody);
        return responseDictionary;

    }
    
    public async Task<Dictionary<string, object>?> SearchStocks(string stockSymbolToSearch)
    {
        //create http request
        HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"https://finnhub.io/api/v1/search?q={stockSymbolToSearch}&token={_configuration["FinnhubToken"]}") //URI includes the secret token
        };

        //send request
        HttpResponseMessage httpResponseMessage = httpClient.Send(httpRequestMessage);

        //read response body
        string responseBody = new StreamReader(httpResponseMessage.Content.ReadAsStream()).ReadToEnd();
        //convert response body (from JSON into Dictionary)
        Dictionary<string, object>? responseDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(responseBody);
        if (responseDictionary != null && responseDictionary.ContainsKey("result"))
        {
            List<Dictionary<string, object>>? result = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(responseDictionary["result"].ToString());
            return result[0];
        }
        return responseDictionary;
    }
}