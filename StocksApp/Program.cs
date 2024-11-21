using Entities;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using Service;
using StocksApp;
using ServiceContract;
using Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient(); //Adding HttpClient for request sending
builder.Services.Configure<TradingOptions>(
builder.Configuration.GetSection("TradingOptions")); // Options pattern
builder.Services.AddScoped<IFinnhubService,FinnhubService>(); //Finnhub service added with scoped timeline
builder.Services.AddScoped<IStocksService,StocksService>(); //StockService service added with scoped timeline
builder.Configuration.AddUserSecrets<Program>();
builder.Services.AddDbContext<StockMarketDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]);
});

var app = builder.Build();
app.UseRotativa();
app.UseStaticFiles();
app.UseRouting();
app.MapControllers();
app.Run();