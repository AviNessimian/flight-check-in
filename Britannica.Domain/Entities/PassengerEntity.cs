using Britannica.Domain.Bases;
using System.Collections.Generic;

namespace Britannica.Domain.Entities
{
    public class PassengerEntity : AuditableEntity
    {
        public int Id { get; set; }

        public string FirtName { get; set; }
        public string LastName { get; set; }

        public ICollection<PassengerFlightEntity> PassengerFlights { get; set; }
    }
}
