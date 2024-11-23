using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ServiceContract;
using StocksApp.Models;

namespace StocksApp.Controllers;

[Route("[controller]")]
public class StocksController : Controller
{
    private static List<Dictionary<string, string>>? stocks;
    private readonly IFinnhubService _finnhubService;
    private readonly TradingOptions _tradingOptions;

    public StocksController(IFinnhubService finnhubService, IOptions<TradingOptions> tradingOptions)
    {
        _finnhubService = finnhubService;
        _tradingOptions = tradingOptions.Value;
        
    }
    
    [Route("/")]
    [Route("[action]/{symbol?}")]
    [Route("~/[action]/{symbol?}")]
    public async Task<IActionResult> Explore(string? symbol,bool showAll = false)
    {
        if (!string.IsNullOrEmpty(HttpContext?.Session.GetString("HasMadeOrder")))
        {
            HttpContext.Session.Remove("HasMadeOrder");   
        }
        if (stocks == null)
        {
            stocks = await _finnhubService.GetStocks();
        }
        if (showAll)
        {
            return View(stocks.Select(temp=>
                new Stock(){ StockSymbol = temp["displaySymbol"],StockName = temp["description"]}).ToList());
        }
        List<string>? list = _tradingOptions.GetStocks();
        List<Stock> x = new List<Stock>();
        foreach (var stock in list)
        {
            Dictionary<string, string>? tmp = stocks.FirstOrDefault(temp => temp["displaySymbol"] == stock);
            if (tmp != null)
            {
                Stock temp = new Stock();
                temp.StockName = tmp["description"];
                temp.StockSymbol = stock;
                x.Add(temp);
            }
        }
        if (symbol != null)
        {
            ViewBag.SelectedSymbol = symbol;
        }
        return View("Explore",x);
    }
}