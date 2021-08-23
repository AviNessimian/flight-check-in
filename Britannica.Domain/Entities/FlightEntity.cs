using Britannica.Domain.Bases;
using System;
using System.Collections.Generic;

namespace Britannica.Domain.Entities
{
    public class FlightEntity : AuditableEntity
    {
        public int Id { get; set; }

        public string Origin { get; set; }
        public string Destination { get; set; }

        public DateTime Depart { get; set; }
        public DateTime Return { get; set; }

        public AircraftEntity Aircraft { get; set; }
        public ICollection<PassengerFlightEntity> PassengerFlights { get; set; }
    }
}
