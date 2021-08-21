using System;

namespace Britannica.Application.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string msg) : base(msg) { }
    }

    
}
