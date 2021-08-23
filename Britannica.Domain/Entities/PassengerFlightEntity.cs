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
                    PassengerFlightSeat = new PassengerFlightSeatEntity
                    {
                        SeatId = seatId
                    }
                };

                newPassengerFlight.Baggages = new List<BaggageEntity>();
                foreach (var bagWeight in baggagesWeight)
                {
                    newPassengerFlight.Baggages.Add(new BaggageEntity
                    {
                        FlightId = flightId,
                        PassengerId = passengerId,
                        Weight = bagWeight,
                    });
                }

                return newPassengerFlight;
            }
        }
    }
}
