using Britannica.Application.Contracts;
using Britannica.Application.Exceptions;
using Britannica.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Britannica.Application.Interactors
{

    public class GetPassengerRequest : IRequest<PassengerEntity>
    {
        public GetPassengerRequest(int id)
        {
            Id = id;
        }
        public int Id { get; }
    }

    internal class GetPassengerInteractor : IRequestHandler<GetPassengerRequest, PassengerEntity>
    {
        private readonly IPassengerRepository _passengerRepository;

        public GetPassengerInteractor(IPassengerRepository passengerRepository)
        {
            _passengerRepository = passengerRepository;
        }

        public async Task<PassengerEntity> Handle(GetPassengerRequest request, CancellationToken cancellationToken)
        {
            var passenger = await _passengerRepository.GetPassenger(request.Id, cancellationToken);
            _ = passenger ?? throw new NotFoundException($"Passenger {passenger.Id} Not Found");
            return passenger;
        }
    }
}
