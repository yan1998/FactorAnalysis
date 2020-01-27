using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class ChangedTypeForUsdAndEur : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "ExchangeRateUSD",
                table: "ExchangeRateFactors",
                type: "Money",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ExchangeRateEUR",
                table: "ExchangeRateFactors",
                type: "Money",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "ExchangeRateUSD",
                table: "ExchangeRateFactors",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "Money");

            migrationBuilder.AlterColumn<decimal>(
                name: "ExchangeRateEUR",
                table: "ExchangeRateFactors",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "Money");
        }
    }
}
