using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Rotativa.AspNetCore;
using Rotativa.AspNetCore.Options;
using ServiceContract;
using StocksApp.Models;
using ServiceContract;
using ServiceContract.DTO;

namespace StocksApp.Controllers;

[Route("[controller]")]
public class TradeController : Controller
{
    private readonly IFinnhubService _finnHubService;
    private readonly IStocksService _stocksService;
    private readonly TradingOptions _options;
    private readonly IConfiguration _configuration;
    
    public TradeController(IFinnhubService finnHubService, IStocksService stocksService, IOptions<TradingOptions> options, IConfiguration configuration)
    {
        _finnHubService = finnHubService;
        _stocksService = stocksService;
        _options = options.Value;
        _configuration = configuration;
    }
    
    
    [Route("/")]
    [Route("[action]")]
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        Dictionary<string,object>? companyProfile = _finnHubService.GetCompanyProfile(_options.Apple);
        Dictionary<string,object>? priceQuote = _finnHubService.GetStockPriceQuote(_options.Apple);
        StocksTrade trade = new StocksTrade()
        {
            StockSymbol = Convert.ToString(companyProfile["ticker"]),
            StockName = Convert.ToString(companyProfile["name"]), 
            Price = Convert.ToDouble(priceQuote["c"].ToString())
        };
        ViewBag.FinnhubToken = _configuration["FinnhubToken"];
        ViewBag.DefaultOrderQuantity = _options.DefaultOrderQuantity;
        return View(trade);
    }
    [HttpPost]
    [Route("[action]")]
    public async Task<IActionResult> SellOrder(SellOrderRequest request,StocksTrade trade)
    {
        request.DateAndTimeOfOrder = DateTime.Now;
        ModelState.Remove("DateAndTimeOfOrder");
        if (!ModelState.IsValid )
        {
            ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            return View("~/Views/Trade/Index.cshtml",trade);
        }
        SellOrderResponse sellOrderResponse = await _stocksService.CreateSellOrder(request);
        return RedirectToAction("Orders"); 
    }
    [HttpPost]
    [Route("[action]")] 
    public async Task<IActionResult> BuyOrder(BuyOrderRequest request,StocksTrade trade)
    {
        request.DateAndTimeOfOrder = DateTime.Now;
        ModelState.Remove("DateAndTimeOfOrder");
        if (!ModelState.IsValid)
        {
            ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            return View("~/Views/Trade/Index.cshtml", trade);
        }
        BuyOrderResponse buyOrderResponse = await _stocksService.CreateBuyOrder(request);
        return RedirectToAction("Orders");
    }
    [Route("[action]")]
    public async Task<IActionResult> Orders()
    {
        List<SellOrderResponse> sellOrderResponses = await _stocksService.GetSellOrders();
        List<BuyOrderResponse> buyOrderResponses = await _stocksService.GetBuyOrders();
        Orders model = new Orders();
        model.SellOrderResponses = sellOrderResponses;
        model.BuyOrderResponses = buyOrderResponses;
        return View(model);
    }
    [HttpGet]
    [Route("[action]")]
    public async Task<IActionResult> OrdersPDF()
    {
        Orders model = new Orders();
        model.BuyOrderResponses = await _stocksService.GetBuyOrders();
        model.SellOrderResponses = await _stocksService.GetSellOrders();
        
        return new ViewAsPdf("OrdersPDF", model)
        {
            PageMargins = new Margins()
            {
                Top = 20,
                Right = 20,
                Bottom = 20,
                Left = 20
            },
            PageOrientation = Orientation.Landscape
        };
    }
}