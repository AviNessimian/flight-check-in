using Britannica.Application.Contracts;
using System;

namespace Britannica.Infrastructure.Services
{
    internal class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.UtcNow;
    }
}
