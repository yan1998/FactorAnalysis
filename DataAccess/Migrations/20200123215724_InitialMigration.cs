using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExchangeRateFactors",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(nullable: false),
                    ExchangeRateUSD = table.Column<decimal>(nullable: false),
                    ExchangeRateEUR = table.Column<decimal>(nullable: false),
                    CreditRate = table.Column<double>(nullable: false),
                    GDPIndicator = table.Column<long>(nullable: false),
                    ImportIndicator = table.Column<double>(nullable: false),
                    ExportIndicator = table.Column<double>(nullable: false),
                    InflationIndex = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExchangeRateFactors", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeRateFactors_Date",
                table: "ExchangeRateFactors",
                column: "Date");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExchangeRateFactors");
        }
    }
}
