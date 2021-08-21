using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Britannica.Domain.Entities
{
    public class PassengerFlightEntity
    {
        public int Id { get; set; }
        public int FlightId { get; set; }

        [JsonIgnore]
        public FlightEntity Flight { get; set; }

        public int PassengerId { get; set; }

        [JsonIgnore]
        public PassengerEntity Passenger { get; set; }

        public PassengerFlightSeatEntity PassengerFlightSeat { get; set; }
        public ICollection<BaggageEntity> Baggages { get; set; }
    }
}
