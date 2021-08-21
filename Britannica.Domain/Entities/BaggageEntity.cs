using Britannica.Domain.Bases;
using System.Text.Json.Serialization;

namespace Britannica.Domain.Entities
{
    public class BaggageEntity : AuditableEntity
    {
        public int Id { get; set; }
        [JsonIgnore]
        public int FlightId { get; set; }
        [JsonIgnore]
        public int PassengerId { get; set; }

        public decimal Weight { get; set; }
        //public string WeightUnits { get; set; }

        [JsonIgnore]
        public PassengerFlightEntity PassengerFlight { get; set; }
    }
}
