using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class MakeDateFieldUnique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ExchangeRateFactors_Date",
                table: "ExchangeRateFactors");

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeRateFactors_Date",
                table: "ExchangeRateFactors",
                column: "Date",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ExchangeRateFactors_Date",
                table: "ExchangeRateFactors");

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeRateFactors_Date",
                table: "ExchangeRateFactors",
                column: "Date");
        }
    }
}
