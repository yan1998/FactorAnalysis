using System;

namespace BusinessLogic.Models
{
    public class ExchangeRateFactors
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public decimal ExchangeRateUSD { get; set; }

        public decimal ExchangeRateEUR { get; set; }

        public double CreditRate { get; set; }

        public long GDPIndicator { get; set; }

        public double ImportIndicator { get; set; }

        public double ExportIndicator { get; set; }

        public double InflationIndex { get; set; }
    }
}
