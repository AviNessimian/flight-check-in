using Britannica.Domain.Bases;
using System.Text.Json.Serialization;

namespace Britannica.Domain.Entities
{
    public class BaggageEntity : AuditableEntity
    {
        public int Id { get; set; }
        public int PassengerFlightRef { get; set; }

        public decimal Weight { get; set; }
        //public string WeightUnits { get; set; }

        [JsonIgnore]
        public PassengerFlightEntity PassengerFlight { get; set; }
    }
}
