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
        public GetPassengerFlightRequest(int id)
        {
            Id = id;
        }
        public int Id { get; set; }
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
            var passengerFlights = await _passengerRepository.GetFlight(request.Id, cancellationToken);
            _ = passengerFlights ?? throw new NotFoundException($"Passenger Flight Not Found");
            return passengerFlights;
        }
    }
}
