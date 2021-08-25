using Britannica.Application.Contracts;
using Britannica.Domain.Entities;
using Britannica.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Britannica.Application.Interactors
{
    public class CheckInRequest : IRequest<PassengerFlightEntity>
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

        [JsonIgnore]
        public int BaggagesCount => Baggages?.Count() ?? 0;

        [JsonIgnore]
        public decimal BaggageTotalWeight => Baggages?.Sum(x => x.Weight) ?? 0;

        [JsonIgnore]
        public decimal[] BaggageWeights => Baggages.Select(x => x.Weight).ToArray();

        public class Baggage { public decimal Weight { get; set; } }
    }

    internal class CheckInInteractor : IRequestHandler<CheckInRequest, PassengerFlightEntity>
    {
        private readonly ILogger<CheckInInteractor> _logger;
        private readonly IFlightRepository _flightRepository;
        private readonly IAircraftRepository _aircraftRepository;
        private readonly IPassengerRepository _passengerRepository;

        public CheckInInteractor(
            ILogger<CheckInInteractor> logger,
            IFlightRepository flightRepository,
            IAircraftRepository aircraftRepository,
            IPassengerRepository passengerRepository)
        {
            _logger = logger;
            _flightRepository = flightRepository;
            _aircraftRepository = aircraftRepository;
            _passengerRepository = passengerRepository;
        }

        public async Task<PassengerFlightEntity> Handle(CheckInRequest request, CancellationToken cancellationToken)
        {
            var flight = await _flightRepository.Get(request.FlightId, cancellationToken);
            _ = flight ?? throw new NotFoundException($"Flight {flight} not found.");

            //1. Aircraft has a limited load weight 
            flight.ValidateAircraftWeightLimit(request.BaggageTotalWeight);

            //2. Aircraft’s seats are limited. Beware of overbooking.
            var seat = await _aircraftRepository.GetSeat(request.SeatId, cancellationToken);
            _ = seat ?? throw new NotFoundException($"Seat {request.SeatId} is not found.");
            seat.ValidateSeatAvailability();

            //3. Each passenger is allowed to check-in a limited number of bags
            flight.ValidateAircraftNumberOfBagsLimit(request.BaggagesCount);

            //4. The total weight of a passenger’s baggage is also limited.
            flight.ValidateTotalPassengerWeightLimit(request.BaggagesCount);

            var newPassengerFlight = PassengerFlightEntity.Factory.Create(
                flightId: request.FlightId,
                passengerId: request.PassengerId,
                seatId: request.SeatId,
                request.BaggageWeights);

            await _passengerRepository.CheckIn(newPassengerFlight, cancellationToken);
            return newPassengerFlight;
        }
    }
}
