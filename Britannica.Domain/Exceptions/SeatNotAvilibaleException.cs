using System;

namespace Britannica.Domain.Exceptions
{
    public class SeatNotAvilibaleException : Exception
    {
        public SeatNotAvilibaleException(string msg) :base(msg) {}
    }
}
