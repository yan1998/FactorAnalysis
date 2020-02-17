using System.Collections.Generic;

namespace DomainModel.ExchangeRateFactors
{
    public class PagedExchangeRateFactors
    {
        public List<ExchangeRateFactors> ExchangeRateFactors { get; set; }

        public int TotalAmount { get; set; }
    }
}
