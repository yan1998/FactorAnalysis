using System;

namespace BusinessLogic.Models
{
    public class ExchangeRateFactors
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public decimal ExchangeRateUSD { get; set; }

        public decimal ExchangeRateEUR { get; set; }

        public float CreditRate { get; set; }

        public long GDPIndicator { get; set; }

        public float ImportIndicator { get; set; }

        public float ExportIndicator { get; set; }

        public float InflationIndex { get; set; }
    }
}
