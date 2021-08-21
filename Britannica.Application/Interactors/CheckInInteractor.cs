using Britannica.Application.Contracts;
using Britannica.Application.Exceptions;
using Britannica.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Britannica.Application.Interactors
{
    public class CheckInRequest : IRequest
    {
        public CheckInRequest() { }
        public CheckInRequest(int flightId, int passangerId, int seatId, List<Baggage> baggages)
        {
            FlightId = flightId;
            PassengerId = passangerId;
            SeatId = seatId;
            Baggages = baggages;
        }
        public int FlightId { get; set; }
        public int PassengerId { get; set; }
        public int SeatId { get; set; }
        public List<Baggage> Baggages { get; set; }

        public class Baggage
        {
            public decimal Weight { get; set; }
        }
    }

    internal class CheckInInteractor : IRequestHandler<CheckInRequest>
    {
        private readonly ILogger<CheckInInteractor> _logger;
        private readonly IFlightRepository _flightRepository;
        private readonly IAircraftRepository _aircraftRepository;
        private readonly IPassengerFlightRepository _passengerFlightRepository;

        public CheckInInteractor(
            ILogger<CheckInInteractor> logger,
            IFlightRepository flightRepository,
            IPassengerFlightRepository passengerFlightRepository,
            IAircraftRepository aircraftRepository)
        {
            _logger = logger;
            _flightRepository = flightRepository;
            _passengerFlightRepository = passengerFlightRepository;
            _aircraftRepository = aircraftRepository;
        }

        public async Task<Unit> Handle(CheckInRequest request, CancellationToken cancellationToken)
        {
            var flight = await _flightRepository.Get(request.FlightId, cancellationToken);
            _ = flight ?? throw new NotFoundException($"Flight {flight} not found.");

            //1. Aircraft has a limited load weight 
            var requestBagsWeight = request.Baggages.Sum(x => x.Weight);
            ValidateAircraftWeightLimit(requestBagsWeight, flight.Aircraft.WeightLimit, flight.PassengerFlights);

            //2. Aircraft’s seats are limited. Beware of overbooking.

            //3. Each passenger is allowed to check-in a limited number of bags
            var requestBaggagesCount = request.Baggages.Count();
            ValidateAircraftNumberOfBagsLimit(requestBaggagesCount, flight.Aircraft.BaggagesLimit, flight.PassengerFlights);

            //4. The total weight of a passenger’s baggage is also limited.
            ValidateTotalPassengerWeightLimit(flight.Aircraft.PassengerBagsLimit, requestBaggagesCount);

            try
            {
                var seat = await _aircraftRepository.GetSeat(request.SeatId, cancellationToken);
                if (seat == null)
                {
                    throw new NotFoundException($"Seat {request.SeatId} is not found.");
                }

                var newPassengerFlight = CreatePassengerFlightEntity(request);
                await _passengerFlightRepository.CheckIn(newPassengerFlight, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new BusinessRuleException(ex.Message);
            }

            return await Task.FromResult(Unit.Value);
        }

        private static void ValidateTotalPassengerWeightLimit(int passengerBagsLimit, int requestBaggagesCount)
        {
            if (requestBaggagesCount > passengerBagsLimit)
            {
                throw new BusinessRuleException($"The total weight of a passenger’s baggage" +
                    $" is limited to {passengerBagsLimit}");
            }
        }

        private static void ValidateAircraftNumberOfBagsLimit(
            int requestBaggagesCount,
            int baggagesLimit,
            ICollection<PassengerFlightEntity> passengerFlights)
        {
            var correntBaggagesCount = 0;
            foreach (var passengerFlight in passengerFlights)
            {
                correntBaggagesCount += passengerFlight.Baggages.Count();
            }

            if ((correntBaggagesCount + requestBaggagesCount) > baggagesLimit)
            {
                throw new BusinessRuleException($"Aircraft limited to {baggagesLimit} number of bags");
            }
        }

        private static void ValidateAircraftWeightLimit(
            decimal requestBagsWeight,
            decimal aircraftWeightLimit,
            ICollection<PassengerFlightEntity> passengerFlights)
        {
            decimal correntAircraftWeight = 0;
            foreach (var passengerFlight in passengerFlights)
            {
                correntAircraftWeight += passengerFlight.Baggages.Sum(x => x.Weight);
            }

            if ((correntAircraftWeight + requestBagsWeight) > aircraftWeightLimit)
            {
                throw new BusinessRuleException($"Aircraft has a limited load weight of {aircraftWeightLimit}");
            }
        }

        private static PassengerFlightEntity CreatePassengerFlightEntity(CheckInRequest request)
        {
            var newPassengerFlight = new PassengerFlightEntity
            {
                FlightId = request.FlightId,
                PassengerId = request.PassengerId,
                PassengerFlightSeat = new PassengerFlightSeatEntity
                {
                    SeatId = request.SeatId
                }
            };

            newPassengerFlight.Baggages = new List<BaggageEntity>();
            foreach (var bag in request.Baggages)
            {
                newPassengerFlight.Baggages.Add(new BaggageEntity
                {
                    FlightId = request.FlightId,
                    PassengerId = request.PassengerId,
                    Weight = bag.Weight,
                });
            }

            return newPassengerFlight;
        }
    }
}
