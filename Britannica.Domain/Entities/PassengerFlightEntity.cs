using Britannica.Domain.Bases;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace Britannica.Domain.Entities
{
    public class PassengerFlightEntity : AuditableEntity
    {
        public int Id { get; set; }

        public int FlightId { get; set; }

        [JsonIgnore]
        public FlightEntity Flight { get; set; }

        public int PassengerId { get; set; }

        [JsonIgnore]
        public PassengerEntity Passenger { get; set; }

        public int SeatId { get; set; }
        public ICollection<BaggageEntity> Baggages { get; set; }

        public int BaggagesCount => Baggages?.Count() ?? 0;

        public decimal BaggagesWeightSum => Baggages?.Sum(x => x.Weight) ?? 0;


        public override string ToString() => 
            $"{nameof(FlightId)}: {FlightId}, " +
            $"{nameof(PassengerId)}: {PassengerId}, " +
            $"{nameof(SeatId)}: {SeatId}, " +
            $"{nameof(BaggagesWeightSum)}: {BaggagesWeightSum}";


        public static class Factory
        {
            public static PassengerFlightEntity Create(
                int flightId,
                int passengerId,
                int seatId,
                decimal[] baggagesWeight)
            {
                var newPassengerFlight = new PassengerFlightEntity
                {
                    FlightId = flightId,
                    PassengerId = passengerId,
                    SeatId = seatId,
                    Baggages = baggagesWeight.Select(weight => new BaggageEntity
                    {
                        Weight = weight
                    }).ToList()
                };

                return newPassengerFlight;
            }
        }
    }
}
