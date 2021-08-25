using Britannica.Domain.Bases;
using Britannica.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

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


        public void ValidateTotalPassengerWeightLimit(int requestBaggagesCount)
        {
            if (requestBaggagesCount > Aircraft.PassengerBagsLimit)
            {
                throw new BusinessRuleException($"The total weight of a passenger’s baggage" +
                    $" is limited to {Aircraft.PassengerBagsLimit}");
            }
        }

        public void ValidateAircraftNumberOfBagsLimit(
            int requestBaggagesCount,
            ICollection<PassengerFlightEntity> passengerFlights)
        {
            var currentBaggagesCount = passengerFlights.Sum(x => x.BaggagesCount);
            if ((currentBaggagesCount + requestBaggagesCount) > Aircraft.BaggagesLimit)
            {
                throw new BusinessRuleException($"Aircraft limited to {Aircraft.BaggagesLimit} number of bags");
            }
        }

        public void ValidateAircraftWeightLimit(
            decimal requestBagsWeight,
            ICollection<PassengerFlightEntity> passengerFlights)
        {
            var currentAircraftWeight = passengerFlights.Sum(x => x.BaggagesWeightSum);
            if ((currentAircraftWeight + requestBagsWeight) > Aircraft.WeightLimit)
            {
                throw new BusinessRuleException($"Aircraft has a limited load weight of {Aircraft.WeightLimit}");
            }
        }
    }
}
