namespace FactorAnalysis.Model.Requests
{
    public class CurrencyExchangePredictionRequest
    {
        public float CreditRate { get; set; }

        public long GDPIndicator { get; set; }

        public float ImportIndicator { get; set; }

        public float ExportIndicator { get; set; }

        public float InflationIndex { get; set; }
    }
}
