using System;

namespace BusinessLogic.Exceptions
{
    public class DomainErrorException : Exception
    {
        public DomainErrorException(string message) : base(message)
        {

        }
    }
}
