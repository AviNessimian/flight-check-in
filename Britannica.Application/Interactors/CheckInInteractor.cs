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
    /// <exception cref="NotFoundException">Thrown when flight or seat not found.</exception>
    /// <exception cref="BusinessRuleException">Thrown when business rule validity.</exception>
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
        private readonly IPassengerRepository _passengerRepository;

        public CheckInInteractor(
            ILogger<CheckInInteractor> logger,
            IFlightRepository flightRepository,
            IPassengerRepository passengerRepository)
        {
            _logger = logger;
            _flightRepository = flightRepository;
            _passengerRepository = passengerRepository;
        }

        public async Task<PassengerFlightEntity> Handle(CheckInRequest request, CancellationToken cancellationToken)
        {
            _logger.LogDebug("1. Aircraft has a limited load weight");
            var flight = await GetFlight(request.FlightId, cancellationToken);
            flight.ValidateAircraftWeightLimit(request.BaggageTotalWeight);

            _logger.LogDebug("2. Aircraft’s seats are limited. Beware of overbooking.");
            var seat = await GetSeat(request.SeatId, cancellationToken);
            seat.ValidateSeatAvailability();

            _logger.LogDebug("3. Each passenger is allowed to check-in a limited number of bags.");
            flight.ValidateAircraftNumberOfBagsLimit(request.BaggagesCount);

            _logger.LogDebug($"4. The total weight of a passenger’s baggage is also limited.");
            flight.ValidateTotalPassengerWeightLimit(request.BaggagesCount);

            var newPassengerFlight = PassengerFlightEntity.Factory.Create(
                flightId: request.FlightId,
                passengerId: request.PassengerId,
                seatId: request.SeatId,
                baggagesWeight: request.BaggageWeights);

            _logger.LogInformation($"Performing Check for {newPassengerFlight}");
            await _passengerRepository.CheckIn(newPassengerFlight, cancellationToken);

            return newPassengerFlight;
        }

        private async Task<SeatEntity> GetSeat(int seatId, CancellationToken cancellationToken)
        {
            var seat = await _flightRepository.GetSeat(seatId, cancellationToken);
            _ = seat ?? throw new NotFoundException($"Seat {seatId} not found.");
            return seat;
        }

        private async Task<FlightEntity> GetFlight(int flightId, CancellationToken cancellationToken)
        {
            var flight = await _flightRepository.Get(flightId, cancellationToken);
            _ = flight ?? throw new NotFoundException($"Flight {flightId} not found.");
            return flight;
        }
    }
}
