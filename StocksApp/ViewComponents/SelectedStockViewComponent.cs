using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RepositoryContracts;
using ServiceContract;

namespace StocksApp.ViewComponents;

[ViewComponent(Name = "SelectedStock")]
public class SelectedStockViewComponent : ViewComponent
{
    private readonly IFinnhubService _finnhubService;

    public SelectedStockViewComponent(IFinnhubService finnhubService)
    {
        _finnhubService = finnhubService;
    }

    public async Task<IViewComponentResult> InvokeAsync(string? stockSymbol)
    {
        Dictionary<string, object>? companyProfileDict = null;
        if (stockSymbol != null)
        {
            companyProfileDict = await _finnhubService.GetCompanyProfile(stockSymbol);
            var stockPriceDict = await _finnhubService.GetStockPriceQuote(stockSymbol);
            if (stockPriceDict != null && companyProfileDict != null)
            {
                companyProfileDict.Add("price", stockPriceDict["c"]);
            }
        }
        if (companyProfileDict != null && companyProfileDict.ContainsKey("logo"))
            return View( "Default",companyProfileDict);
        else
            return Content("");
    }
}