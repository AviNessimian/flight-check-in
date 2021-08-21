using Britannica.Application.Contracts;
using Britannica.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Britannica.Application.Interactors
{

    public class GetPassengerFlightRequest : IRequest<PassengerEntity>
    {
        public GetPassengerFlightRequest(int id)
        {
            Id = id;
        }
        public int Id { get; }
    }

    internal class GetPassengerFlightInteractor : IRequestHandler<GetPassengerRequest, PassengerEntity>
    {
        private readonly IPassengerFlightRepository _passengerFlightRepository;

        public GetPassengerFlightInteractor(IPassengerFlightRepository passengerFlightRepository)
        {
            _passengerFlightRepository = passengerFlightRepository;
        }

        public async Task<PassengerEntity> Handle(GetPassengerRequest request, CancellationToken cancellationToken)
        {
            //var passenger = await _passengerFlightRepository.Get(request.Id, cancellationToken);
            //_ = passenger ?? throw new NotFoundException($"Passenger {passenger.Id} Not Found");
            //return passenger;

            return null;
        }
    }
}
