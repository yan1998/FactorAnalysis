using System;

namespace DomainModel.ExchangeRateFactors
{
    public class SeedFileDataRange<T>
    {
        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }

        public T Value { get; set; }
    }
}
