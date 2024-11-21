using Microsoft.EntityFrameworkCore;

namespace Entities;

public class StockMarketDbContext : DbContext 
{
    public DbSet<BuyOrder> BuyOrders { get; set; }
    public DbSet<SellOrder> SellOrders { get; set; }

    public StockMarketDbContext(DbContextOptions<StockMarketDbContext> options) : base(options)
    {
        
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<BuyOrder>().ToTable("BuyOrders");
        modelBuilder.Entity<SellOrder>().ToTable("SellOrders");
        //Seeding
        modelBuilder.Entity<BuyOrder>().HasData(new BuyOrder()
        {
            BuyOrderID = Guid.Parse("3f0e2d80-57a8-42e1-804b-3038a5fbdaca"),
            StockName = "Microsoft Corp",
            StockSymbol = "MSFT",
            DateAndTimeOfOrder = Convert.ToDateTime("2021-11-01 07:51:00"),
            Quantity = 8,
            Price = 336.32,
            
        });
        modelBuilder.Entity<BuyOrder>().HasData(new BuyOrder()
        {
            BuyOrderID = Guid.Parse("c9e8a5a4-0d49-4c19-a9f6-5f29bcb44731"),
            StockName = "Microsoft Corp",
            StockSymbol = "MSFT",
            DateAndTimeOfOrder = Convert.ToDateTime("2009-3-12 01:33:00"),
            Quantity = 21,
            Price = 23.00,
        });
        modelBuilder.Entity<SellOrder>().HasData(new SellOrder()
        {
            SellOrderID = Guid.Parse("e45dbd7c-23e4-4d74-83cb-f84ae4f85f50"),
            DateAndTimeOfOrder = Convert.ToDateTime("2021-9-1 07:51:00"),
            Price = 336.32,
            Quantity = 1,
            StockSymbol = "MSFT",
            StockName = "Microsoft Corp",
        });
    }
}