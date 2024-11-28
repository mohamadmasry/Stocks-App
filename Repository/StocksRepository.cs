using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;
using Serilog;

namespace Repository;

public class StocksRepository(StockMarketDbContext context, IDiagnosticContext diagnosticContext)
    : IStocksRepository
{
    public async Task<BuyOrder> CreateBuyOrder(BuyOrder buyOrder)
    {
        await context.BuyOrders.AddAsync(buyOrder);
        await context.SaveChangesAsync();
        diagnosticContext.Set("Created buy order", buyOrder);
        return buyOrder;
    }

    public async Task<SellOrder> CreateSellOrder(SellOrder sellOrder)
    {
        await context.SellOrders.AddAsync(sellOrder);
        await context.SaveChangesAsync();
        diagnosticContext.Set("Created sell order", sellOrder);
        return sellOrder;
    }

    public async Task<List<BuyOrder>> GetBuyOrders()
    {
        List<BuyOrder> result = await context.BuyOrders.ToListAsync();
        diagnosticContext.Set("Buy orders list",result);
        return result;

    }

    public async Task<List<SellOrder>> GetSellOrders()
    {
        List<SellOrder> result =  await context.SellOrders.ToListAsync();
        diagnosticContext.Set("Sell orders list",result);
        return result;
    }
}