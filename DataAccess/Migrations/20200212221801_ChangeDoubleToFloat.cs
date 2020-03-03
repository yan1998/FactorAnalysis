using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class ChangeDoubleToFloat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "InflationIndex",
                table: "ExchangeRateFactors",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<float>(
                name: "ImportIndicator",
                table: "ExchangeRateFactors",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<float>(
                name: "ExportIndicator",
                table: "ExchangeRateFactors",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<float>(
                name: "CreditRate",
                table: "ExchangeRateFactors",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "InflationIndex",
                table: "ExchangeRateFactors",
                type: "float",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<double>(
                name: "ImportIndicator",
                table: "ExchangeRateFactors",
                type: "float",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<double>(
                name: "ExportIndicator",
                table: "ExchangeRateFactors",
                type: "float",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<double>(
                name: "CreditRate",
                table: "ExchangeRateFactors",
                type: "float",
                nullable: false,
                oldClrType: typeof(float));
        }
    }
}
