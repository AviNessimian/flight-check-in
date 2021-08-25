using System;

namespace Britannica.Domain.Exceptions
{
    public class BusinessRuleException : Exception
    {
        public BusinessRuleException(string msg) : base(msg) { }
    } 
}
