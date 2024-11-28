using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Rotativa.AspNetCore;
using Rotativa.AspNetCore.Options;
using ServiceContract;
using StocksApp.Models;
using ServiceContract.DTO;

namespace StocksApp.Controllers;

[Route("[controller]")]
public class TradeController : Controller
{
    private readonly IFinnhubService _finnHubService;
    private readonly IStocksService _stocksService;
    private readonly TradingOptions _options;
    private readonly IConfiguration _configuration;
    private readonly ILogger<TradeController> _logger;
    
    public TradeController(IFinnhubService finnHubService, IStocksService stocksService, IOptions<TradingOptions> options,
        IConfiguration configuration, ILogger<TradeController> logger)
    {
        _logger = logger;
        _finnHubService = finnHubService;
        _stocksService = stocksService;
        _options = options.Value;
        _configuration = configuration;
    }
    
    
    [Route("[action]/{stockSymbol?}")]
    [HttpGet]
    public async Task<IActionResult> Index(String? stockSymbol)
    {
        string? HasMadeOrder = HttpContext.Session.GetString("HasMadeOrder");
        if (!string.IsNullOrEmpty(HasMadeOrder) && HasMadeOrder.Equals("true") )
        {
            _logger.LogWarning("Can't make more than one order at a time.");
            return RedirectToAction("Orders");
        }
        Dictionary<string,object>? companyProfile = await _finnHubService.GetCompanyProfile(stockSymbol ?? "MSFT");
        Dictionary<string,object>? priceQuote = await _finnHubService.GetStockPriceQuote(stockSymbol ?? "MSFT");
        StocksTrade trade = new StocksTrade()
        {
            StockSymbol = Convert.ToString(companyProfile["ticker"]),
            StockName = Convert.ToString(companyProfile["name"]), 
            Price = Convert.ToDouble(priceQuote["c"].ToString())
        };
        ViewBag.FinnhubToken = _configuration["FinnhubToken"];
        ViewBag.DefaultOrderQuantity = _options.DefaultOrderQuantity;
        ViewBag.CurrentUrl = "~/Trade/Index";
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
            ViewBag.CurrentUrl = "~/Trade/Index";
            _logger.LogWarning($"ModelState is not valid: {ViewBag.Errors}");
            return View("~/Views/Trade/Index.cshtml",trade);
        }
        await _stocksService.CreateSellOrder(request);
        return RedirectToActionPermanent("Orders");
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
            ViewBag.CurrentUrl = "~/Trade/Index";
            _logger.LogWarning($"ModelState is not valid: {ViewBag.Errors}");
            return View("~/Views/Trade/Index.cshtml", trade);
        }
        await _stocksService.CreateBuyOrder(request);
        return RedirectToActionPermanent("Orders");
    }
    [Route("[action]")]
    public async Task<IActionResult> Orders()
    {
        HttpContext.Session.SetString("HasMadeOrder", "true");
        List<SellOrderResponse> sellOrderResponses = await _stocksService.GetSellOrders();
        List<BuyOrderResponse> buyOrderResponses = await _stocksService.GetBuyOrders();
        Orders model = new Orders();
        model.SellOrderResponses = sellOrderResponses;
        model.BuyOrderResponses = buyOrderResponses;
        ViewBag.CurrentUrl = "~/Trade/Orders";
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