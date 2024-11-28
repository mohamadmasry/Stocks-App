using Entities;
using Microsoft.EntityFrameworkCore;
using Repository;
using RepositoryContracts;
using Rotativa.AspNetCore;
using Serilog;
using Service;
using StocksApp;
using ServiceContract;
using Service;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((context, services, config) =>
{
    config
        .ReadFrom.Services(services)
        .ReadFrom.Configuration(context.Configuration);
});
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient(); //Adding HttpClient for request sending
builder.Services.Configure<TradingOptions>(builder.Configuration.GetSection("TradingOptions")); // Options pattern
builder.Services.AddScoped<IFinnhubService,FinnhubService>(); //Finnhub service added with scoped timeline
builder.Services.AddScoped<IStocksService,StocksService>(); //StockService service added with scoped timeline
builder.Services.AddScoped<IFinnhubRepository, FinnhubRepository>(); //Finnhub repository added with scoped timeline
builder.Services.AddScoped<IStocksRepository, StocksRepository>(); //Stock repository added with scoped timeline
builder.Configuration.AddUserSecrets<Program>(); //Adding user-secrets
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<StockMarketDbContext>(options =>
    {
        options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]);
    });
}
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
});
var app = builder.Build();
app.UseSerilogRequestLogging();
app.UseSession();
app.UseRotativa();
app.UseStaticFiles();
app.UseRouting();
app.MapControllers();
app.Run();

public partial class Program { }