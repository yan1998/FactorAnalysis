using System;

namespace DomainModel.ExchangeRateFactors
{
    public class SeedFileData<T>
    {
        public DateTime Date { get; set; }

        public T Value { get; set; }
    }
}
