using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BuyOrders",
                columns: table => new
                {
                    BuyOrderID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StockSymbol = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StockName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateAndTimeOfOrder = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuyOrders", x => x.BuyOrderID);
                });

            migrationBuilder.CreateTable(
                name: "SellOrders",
                columns: table => new
                {
                    SellOrderID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StockSymbol = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StockName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateAndTimeOfOrder = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SellOrders", x => x.SellOrderID);
                });

            migrationBuilder.InsertData(
                table: "BuyOrders",
                columns: new[] { "BuyOrderID", "DateAndTimeOfOrder", "Price", "Quantity", "StockName", "StockSymbol" },
                values: new object[,]
                {
                    { new Guid("3f0e2d80-57a8-42e1-804b-3038a5fbdaca"), new DateTime(2021, 11, 1, 7, 51, 0, 0, DateTimeKind.Unspecified), 336.31999999999999, 8, "Microsoft Corp", "MSFT" },
                    { new Guid("c9e8a5a4-0d49-4c19-a9f6-5f29bcb44731"), new DateTime(2009, 3, 12, 1, 33, 0, 0, DateTimeKind.Unspecified), 23.0, 21, "Microsoft Corp", "MSFT" }
                });

            migrationBuilder.InsertData(
                table: "SellOrders",
                columns: new[] { "SellOrderID", "DateAndTimeOfOrder", "Price", "Quantity", "StockName", "StockSymbol" },
                values: new object[] { new Guid("e45dbd7c-23e4-4d74-83cb-f84ae4f85f50"), new DateTime(2021, 9, 1, 7, 51, 0, 0, DateTimeKind.Unspecified), 336.31999999999999, 1, "Microsoft Corp", "MSFT" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BuyOrders");

            migrationBuilder.DropTable(
                name: "SellOrders");
        }
    }
}
