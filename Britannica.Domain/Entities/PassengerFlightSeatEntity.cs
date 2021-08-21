using System.Text.Json.Serialization;

namespace Britannica.Domain.Entities
{
    public class PassengerFlightSeatEntity
    {
        public int FlightId { get; set; }
        public int PassengerId { get; set; }

        [JsonIgnore]
        public PassengerFlightEntity PassengerFlight { get; set; }

        public int SeatId { get; set; }
    }
   
}
