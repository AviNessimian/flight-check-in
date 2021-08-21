using Britannica.Application.Contracts;
using Britannica.Application.Exceptions;
using Britannica.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Britannica.Application.Interactors
{

    public class GetFlightRequest : IRequest<FlightEntity>
    {
        public GetFlightRequest(int id)
        {
            Id = id;
        }
        public int Id { get; }
    }

    internal class GetFlightInteractor : IRequestHandler<GetFlightRequest, FlightEntity>
    {
        private readonly IFlightRepository _flightRepository;

        public GetFlightInteractor(IFlightRepository flightRepository)
        {
            _flightRepository = flightRepository;
        }

        public async Task<FlightEntity> Handle(GetFlightRequest request, CancellationToken cancellationToken)
        {
            var flight = await _flightRepository.Get(request.Id, cancellationToken);
            _ = flight ?? throw new NotFoundException($"Flight number {request.Id} Not Found");
            return flight;
        }
    }
}
