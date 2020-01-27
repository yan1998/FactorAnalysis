using System;

namespace BusinessLogic.Models
{
    public class SeedFileData<T>
    {
        public DateTime Date { get; set; }

        public T Value { get; set; }
    }
}
