using System;

namespace Britannica.Application.Exceptions
{
    public class AppBusinessRuleException : Exception
    {
        public AppBusinessRuleException(string msg) : base(msg) { }
    }

    
}
