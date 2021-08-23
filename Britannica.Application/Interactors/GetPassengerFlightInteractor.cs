using Britannica.Application.Contracts;
using Britannica.Application.Exceptions;
using Britannica.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Britannica.Application.Interactors
{

    public class GetPassengerFlightRequest : IRequest<PassengerFlightEntity>
    {
        public GetPassengerFlightRequest(int flightId, int passengerId)
        {
            FlightId = flightId;
            PassengerId = passengerId;
        }
        public int FlightId { get; set; }

        public int PassengerId { get; set; }

    }

    internal class GetPassengerFlightInteractor : IRequestHandler<GetPassengerFlightRequest, PassengerFlightEntity>
    {
        private readonly IPassengerRepository _passengerRepository;

        public GetPassengerFlightInteractor(IPassengerRepository passengerRepository)
        {
            _passengerRepository = passengerRepository;
        }

        public async Task<PassengerFlightEntity> Handle(GetPassengerFlightRequest request, CancellationToken cancellationToken)
        {
            var passengerFlights = await _passengerRepository.Get(request.FlightId, request.PassengerId, cancellationToken);
            _ = passengerFlights ?? throw new NotFoundException($"Passenger Flight Not Found");
            return passengerFlights;
        }
    }
}
